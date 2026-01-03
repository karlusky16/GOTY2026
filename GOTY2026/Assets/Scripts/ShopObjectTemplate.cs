using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopObjectTemplate : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;
    public DisplayItems displayItem;   // Componente que muestra la carta
    public Button buyButton;          
    public TMP_Text priceText;        

    private int itemId;
    private int price;
    private ShopManager manager;
    
    public void Setup(int id, int price, ShopManager shopManager)
    {
        itemId = id;
        this.price = price;
        manager = shopManager;

        if (displayItem != null)
            displayItem.ActualizarID(itemId);

        if (priceText != null)
        {
            if (price == 0)
            {
                priceText.text = "Comprado";
                return;
            }
            priceText.text = " Monedas: " + price.ToString();
        }
        
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => manager.ComprarItem(itemId, price, displayItem.item._name));
        buyButton.onClick.AddListener(() => audioSource.PlayOneShot(pulsarBotonClip));

        int dineroActual = GameManager.player.GetComponent<PlayerController>().GetMonedas();
        UpdateInteractivity(dineroActual);

    }

    public void UpdateInteractivity(int dineroActual)
    {
        bool canAfford = dineroActual >= price;
        buyButton.interactable = canAfford;
    }
}

