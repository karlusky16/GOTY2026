using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject ItemsPanel;
    public GameObject CardsPanel;
    public Button ItemsBtn;
    public Button CardsBtn;
    
    public GameObject shopCardTemplate;

    public Transform panelCartasTienda;
    public ScrollRect scrollRectCartasTienda;
    public TMP_Text monedasText;

    public GameObject shopPanel;

    public GameObject MensajeCompraSatisfactoria;
    public TMP_Text textoMensajeCompraSatisfactoria;

    public void Start()
    {
        CerrarTienda();
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
        ItemsBtn.interactable = true;
        CardsBtn.interactable = false;
        CardsPanel.SetActive(true);
        ItemsPanel.SetActive(false);

        MensajeCompraSatisfactoria.SetActive(false);
        MostrarCartasEnTienda();
        Canvas.ForceUpdateCanvases();
        scrollRectCartasTienda.verticalNormalizedPosition = 1f;
    }
    public void ItemsPanelActivate()
    {
        ItemsBtn.interactable = false;
        CardsBtn.interactable = true;
        ItemsPanel.SetActive(true);
        CardsPanel.SetActive(false);
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

            CardShopTemplate storeItem = nuevoItem.GetComponent<CardShopTemplate>();
            if (storeItem != null)
            {
                storeItem.Setup(cardId, precio, this);
            }
            else
            {
                Debug.LogWarning("El prefab no tiene el componente CardShopTemplate.");
            }
        }
        RefrescarInteractividadDeTodos();
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
    }

    /// Recalcula la interactividad de todos los items según el dinero actual.
    private void RefrescarInteractividadDeTodos()
    {
        // Recorre todos los hijos del panel y llama a UpdateInteractivity() si tienen CardShopTemplate
        foreach (Transform t in panelCartasTienda)
        {
            var item = t.GetComponent<CardShopTemplate>();
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
        RefrescarInteractividadDeTodos();
    }
}
