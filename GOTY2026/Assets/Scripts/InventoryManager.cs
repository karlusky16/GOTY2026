using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;
    public GameObject ItemsPanel;
    public GameObject CardsPanel;
    public Button ItemsBtn;
    public Button CardsBtn;
    
    public GameObject inventoryCardTemplate;
    public GameObject inventoryObjectTemplate;

    public Transform panelCartasInventario;
    public Transform panelObjetosInventario;

    public ScrollRect scrollRectCartasInventario;
    public ScrollRect scrollRectObjetosInventario;
    public TMP_Text monedasText;
    public GameObject inventoryPanel;

    public void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void AbrirInventario()
    {
        inventoryPanel.SetActive(true);
        inventoryPanel.GetComponent<InventoryManager>().ActualizarMonedasUI();
        inventoryPanel.GetComponent<InventoryManager>().CardsPanelActivate();
    }
    public void CerrarInventario()
    {
        inventoryPanel.SetActive(false);
    }
    public void CardsPanelActivate()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        ItemsBtn.interactable = true;
        CardsBtn.interactable = false;
        CardsPanel.SetActive(true);
        ItemsPanel.SetActive(false);
        
        MostrarCartasEnInventario();
        Canvas.ForceUpdateCanvases();
        scrollRectCartasInventario.verticalNormalizedPosition = 1f;
    }
    public void ItemsPanelActivate()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        ItemsBtn.interactable = false;
        CardsBtn.interactable = true;
        ItemsPanel.SetActive(true);
        CardsPanel.SetActive(false);

        MostrarObjetosEnInventario();
        Canvas.ForceUpdateCanvases();
        scrollRectObjetosInventario.verticalNormalizedPosition = 1f;
    }

    public void MostrarCartasEnInventario()
    {
        if (PlayerController.cartas == null || PlayerController.cartas.Count == 0)
        {
            Debug.LogWarning("La lista de cartas de player está vacía o no inicializada.");
            return;
        }

        if (GameManager.cardList == null || GameManager.cardList.Count == 0)
        {
            Debug.LogError("La lista de cartas del manager está vacía o no inicializada.");
            return;
        }

        if (inventoryCardTemplate == null || panelCartasInventario == null)
        {
            Debug.LogError("Prefab o panel no asignados en el Inspector.");
            return;
        }

        // Limpiar el panel
        for (int i = panelCartasInventario.childCount - 1; i >= 0; i--)
        {
            Destroy(panelCartasInventario.GetChild(i).gameObject);
        }
        
        // Agrupar cartas por ID y contar cantidades
        var grupos = PlayerController.cartas.GroupBy(id => id).OrderBy(g => g.Key); // Key es el ID

        foreach (var grupo in grupos)
        {
            int cardId = grupo.Key;
            int cantidad = grupo.Count();
            

            var carta = GameManager.cardList.Find(c => c.id == cardId);
            if (carta == null)
            {
                Debug.LogError("Carta no encontrada con ID: " + cardId);
                continue;
            }

            GameObject nuevoItem = Instantiate(inventoryCardTemplate, panelCartasInventario);
            var inventoryItem = nuevoItem.GetComponent<InventoryCardTemplate>();
            if (inventoryItem != null)
            {
                inventoryItem.Setup(cardId, cantidad);
            }
        }
        ActualizarMonedasUI();
    }

    public void MostrarObjetosEnInventario()
    {
        if(PlayerController.pasivos == null || PlayerController.pasivos.Count == 0)
        {
            Debug.LogWarning("La lista de objetos del player está vacía o no inicializada.");
            return;
        }

        if(GameManager.itemsLis == null || GameManager.itemsLis.Count == 0)
        {
            Debug.LogWarning("La lista de objetos del manager está vacía o no inicializada.");
            return;
        }
        if(inventoryObjectTemplate == null || panelObjetosInventario == null)
        {
            Debug.LogError("Prefab o panel no asignados en el Inspector.");
            return;
        }
        int nObj = PlayerController.pasivos.Count;
        Debug.Log("nObj: "+ nObj);
        //limiar el panel
        for(int i = panelObjetosInventario.childCount-1; i>=0; i--)
        {
            Destroy(panelObjetosInventario.GetChild(i).gameObject);
        }

        //agrupar objetos por id y contar cantidades
        var grupos = PlayerController.pasivos.GroupBy(id => id).OrderBy(g=> g.Key);

        foreach(var grupo in grupos)
        {
            int objetId = grupo.Key;
            int cantidad = grupo.Count();
            Debug.Log("ObjetoID: " + objetId);

            var objeto = GameManager.itemsLis.Find(c => c.id == objetId);
            if(objeto == null)
            {
                Debug.LogError("Objeto no encontrado con ID: " +objetId);
                continue;
            }

            GameObject nuevoItem = Instantiate(inventoryObjectTemplate, panelObjetosInventario);
            var inventoryItem = nuevoItem.GetComponent<InventoryObjectTemplate>();
            if(inventoryItem != null)
            {
                inventoryItem.Setup(objetId, cantidad);
            }
        }
        ActualizarMonedasUI();
    }

    public void ActualizarMonedasUI()
    {
        int monedasJugador = GameManager.player.GetComponent<PlayerController>().GetMonedas();
        monedasText.text = "Monedas: " + monedasJugador.ToString();
    }
}
