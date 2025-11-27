using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCardTemplate : MonoBehaviour
{
    public DisplayCard displayCard;   // Componente que muestra la carta
    public TMP_Text cantidadText;       
    private int cardId;
    private int cantidad;

    
    public void Setup(int id, int count)
    {
        cardId = id;
        cantidad = count;

        if (displayCard != null)
            displayCard.ActualizarID(cardId);

        
        if (cantidadText != null)
        {
            cantidadText.text = "x" + cantidad.ToString();
        }
    }

}

