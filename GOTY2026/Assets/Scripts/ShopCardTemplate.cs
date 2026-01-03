using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardTemplate : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;

    public DisplayCard displayCard;   // Componente que muestra la carta
    public Button buyButton;          
    public TMP_Text priceText;        

    private int cardId;
    private int price;
    private ShopManager manager;
    
    public void Setup(int id, int price, ShopManager shopManager)
    {
        cardId = id;
        this.price = price;
        manager = shopManager;

        if (displayCard != null)
            displayCard.ActualizarID(cardId);

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
        buyButton.onClick.AddListener(() => manager.ComprarCarta(cardId, price, displayCard.card._name));
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

