using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DisplayItems : MonoBehaviour
{
    public ItemPasivo item;
    public int displayID;
    public Image itemImage;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemTextNombre;
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        item = GameManager.itemsLis[displayID];
        itemImage.sprite = item.sprite;
        itemImage.GetComponent<Image>().SetNativeSize();
        itemText.text = item.descripcion;
        itemTextNombre.text = item._name;
    }
}
