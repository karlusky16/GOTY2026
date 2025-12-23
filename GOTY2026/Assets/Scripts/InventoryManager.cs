using System;
using System.Linq;
using TMPro;
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

    public Transform panelCartasInventario;
    public ScrollRect scrollRectCartasInventario;
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

    public void ActualizarMonedasUI()
    {
        int monedasJugador = GameManager.player.GetComponent<PlayerController>().GetMonedas();
        monedasText.text = "Monedas: " + monedasJugador.ToString();
    }
}
