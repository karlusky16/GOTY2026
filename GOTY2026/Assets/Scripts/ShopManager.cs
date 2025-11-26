using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    
    public TextMeshProUGUI Shop;
    public GameObject ItemsPanel;
    public GameObject CardsPanel;
    public Button ItemsBtn;
    public Button CardsBtn;
    
    public GameObject shopCardTemplate;

    public Transform panelCartasTienda;
    public ScrollRect scrollRectCartasTienda;
    public TMP_Text monedasText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActualizarMonedasUI();
        ItemsActivate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizarMonedasUI()
    {
        int monedasJugador = GameManager.player.GetComponent<PlayerController>().GetMonedas();
        monedasText.text = "Monedas: " + monedasJugador.ToString();
        RefrescarInteractividadDeTodos();
    }
    
    public void CardsActivate()
    {
        ItemsBtn.interactable = true;
        CardsBtn.interactable = false;
        CardsPanel.SetActive(true);
        ItemsPanel.SetActive(false);
        MostrarCartasEnTienda();
        //Canvas.ForceUpdateCanvases();
        scrollRectCartasTienda.verticalNormalizedPosition = 1f;
    }
    public void ItemsActivate()
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

    public void ComprarCarta(int cardId, int precio)
    {
        PlayerController player = GameManager.player.GetComponent<PlayerController>();
        if (player.GetMonedas() >= precio)
        {
            player.ReducirMonedas(precio);
            player.AddCarta(cardId);
            ActualizarMonedasUI();
            Debug.Log("Carta comprada por " + precio + " monedas.");
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

    public void SigEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
}
