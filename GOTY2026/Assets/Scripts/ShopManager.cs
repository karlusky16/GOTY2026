using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;

    public GameObject ItemsPanel;
    public GameObject CardsPanel;

    public Button ItemsBtn;
    public Button CardsBtn;
    
    public GameObject shopCardTemplate;
    public GameObject shopObjectTemplate;

    public Transform panelCartasTienda;
    public ScrollRect scrollRectCartasTienda;
    public Transform panelObjetosTienda;
    public ScrollRect scrollRectObjetosTienda;

    public TMP_Text monedasText;

    public GameObject shopPanel;

    public GameObject MensajeCompraSatisfactoria;
    public TMP_Text textoMensajeCompraSatisfactoria;
    public GameObject MensajeErrorPasivos;
    public TMP_Text textoMensajeErrorPasivos;

    public void Start()
    {
        shopPanel.SetActive(false);
    }
    public void AbrirTienda()
    {
        shopPanel.SetActive(true);
        shopPanel.GetComponent<ShopManager>().ActualizarMonedasUI();
        shopPanel.GetComponent<ShopManager>().CardsPanelActivate();
    }
    public void CerrarTienda()
    {
        shopPanel.SetActive(false);
    }
    public void CardsPanelActivate()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        ItemsBtn.interactable = true;
        CardsBtn.interactable = false;
        CardsPanel.SetActive(true);
        ItemsPanel.SetActive(false);

        MensajeCompraSatisfactoria.SetActive(false);
        MensajeErrorPasivos.SetActive(false);
        MostrarCartasEnTienda();
        Canvas.ForceUpdateCanvases();
        scrollRectCartasTienda.verticalNormalizedPosition = 1f;
    }
    public void ItemsPanelActivate()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        ItemsBtn.interactable = false;
        CardsBtn.interactable = true;
        ItemsPanel.SetActive(true);
        CardsPanel.SetActive(false);

        MensajeCompraSatisfactoria.SetActive(false);
        MensajeErrorPasivos.SetActive(false);
        MostrarObjetosEnTienda();
        Canvas.ForceUpdateCanvases();
        scrollRectObjetosTienda.verticalNormalizedPosition = 1f;
    }

    public void MostrarCartasEnTienda()
    {
        if (GameManager.cardList == null || GameManager.cardList.Count == 0)
        {
            Debug.LogError("La lista de cartas está vacía o no inicializada.");
            return;
        }

        if (shopCardTemplate == null || panelCartasTienda == null)
        {
            Debug.LogError("Prefab o panel no asignados en el Inspector.");
            return;
        }

        // Limpiar el panel
        for (int i = panelCartasTienda.childCount - 1; i >= 0; i--)
        {
            Destroy(panelCartasTienda.GetChild(i).gameObject);
        }

        // Instanciar cada carta
        for (int i = 0; i < GameManager.cardList.Count; i++)
        {
            int cardId = GameManager.cardList[i].id;
            int precio = GameManager.cardList[i].precio;

            GameObject nuevoItem = Instantiate(shopCardTemplate, panelCartasTienda);

            ShopCardTemplate storeItem = nuevoItem.GetComponent<ShopCardTemplate>();
            if (storeItem != null)
            {
                storeItem.Setup(cardId, precio, this);
            }
            else
            {
                Debug.LogWarning("El prefab no tiene el componente CardShopTemplate.");
            }
        }
        RefrescarInteractividadDeTodosShop();
    }

    public void MostrarObjetosEnTienda()
    {
        if(GameManager.itemsLis == null || GameManager.itemsLis.Count == 0)
        {
            Debug.LogError("La lista de objetos está vacía o no inicializada.");
            return;
        }
        if(shopObjectTemplate == null || panelObjetosTienda == null)
        {
            Debug.LogError("Prefab o panel no asignados en el Inspector.");
            return;
        }
        for(int i = panelObjetosTienda.childCount -1; i>=0; i--)
        {
            Destroy(panelObjetosTienda.GetChild(i).gameObject);
        }

        for(int i = 0; i< GameManager.itemsLis.Count; i++)
        {
            int itemId = GameManager.itemsLis[i].id;
            int precio = GameManager.itemsLis[i].precio;

            GameObject nuevoItem = Instantiate(shopObjectTemplate, panelObjetosTienda);

            ShopObjectTemplate storeItem = nuevoItem.GetComponent<ShopObjectTemplate>();
            if (storeItem != null)
            {
                storeItem.Setup(itemId, precio, this);
            }
            else
            {
                Debug.LogWarning("El prefab no tiene el componente ShopObjectTemplate.");
            }
        }
        RefrescarInteractividadDeTodosShop();
    }

    public void ComprarCarta(int cardId, int precio, String nombreCarta)
    {
        PlayerController player = GameManager.player.GetComponent<PlayerController>();
        if (player.GetMonedas() >= precio)
        {
            Debug.Log(nombreCarta);
            player.ReducirMonedas(precio);
            player.AddCarta(cardId);

            textoMensajeCompraSatisfactoria.text ="La carta " + nombreCarta + " comprada por " + precio + " monedas ha sido añadida a tu inventario.";
            MensajeCompraSatisfactoria.SetActive(true);

            ActualizarMonedasUI();
        }
        RefrescarInteractividadDeTodosShop();
    }
    public void ComprarItem(int itemId, int precio, String nombreItem)
    {
        bool flag = false;
        PlayerController player = GameManager.player.GetComponent<PlayerController>();
        for(int i=0; i<PlayerController.pasivos.Count; i++)
        {
            int id = PlayerController.pasivos[i];
            if(id == itemId)
            {
                textoMensajeErrorPasivos.text ="El objeto " + nombreItem + " solo se puede tener una vez en el inventario.";
                MensajeErrorPasivos.SetActive(true);
                flag = true;
            }
        }
        if (player.GetMonedas() >= precio && flag == false)
        {
            Debug.Log(nombreItem);
            player.ReducirMonedas(precio);
            player.AddPasivo(itemId);

            textoMensajeCompraSatisfactoria.text ="El objeto " + nombreItem + " comprado por " + precio + " monedas ha sido añadida a tu inventario.";
            MensajeCompraSatisfactoria.SetActive(true);

            ActualizarMonedasUI();
        }
        RefrescarInteractividadDeTodosInventory();
    }

    /// Recalcula la interactividad de todos los items según el dinero actual.
    private void RefrescarInteractividadDeTodosShop()
    {
        // Recorre todos los hijos del panel y llama a UpdateInteractivity() si tienen CardShopTemplate
        foreach (Transform t in panelCartasTienda)
        {
            var item = t.GetComponent<ShopCardTemplate>();
            if (item != null)
            {
                item.UpdateInteractivity(GameManager.player.GetComponent<PlayerController>().GetMonedas());
            }
        }
    }

    private void RefrescarInteractividadDeTodosInventory()
    {
        // Recorre todos los hijos del panel y llama a UpdateInteractivity() si tienen CardShopTemplate
        foreach (Transform t in panelObjetosTienda)
        {
            var item = t.GetComponent<ShopObjectTemplate>();
            if (item != null)
            {
                item.UpdateInteractivity(GameManager.player.GetComponent<PlayerController>().GetMonedas());
            }
        }
    }

    public void ActualizarMonedasUI()
    {
        int monedasJugador = GameManager.player.GetComponent<PlayerController>().GetMonedas();
        monedasText.text = "Monedas: " + monedasJugador.ToString();
    }
}
