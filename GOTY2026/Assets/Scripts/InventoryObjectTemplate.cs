using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryObjectTemplate : MonoBehaviour
{
    public DisplayItems displayItem;   // Componente que muestra la carta
    public TMP_Text cantidadText;       
    private int itemId;
    private int cantidad;

    
    public void Setup(int id, int count)
    {
        itemId = id;
        cantidad = count;

        if (displayItem != null)
            displayItem.ActualizarID(itemId);

        if (cantidadText != null)
        {
            cantidadText.text = "x" + cantidad.ToString();
        }
    }

}

