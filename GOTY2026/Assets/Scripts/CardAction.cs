using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardAction : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image borde;
    public GameObject carta;
    Vector3 posicion;
    Vector3 scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicion = carta.transform.position;
        scale = carta.transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            borde.color = Color.red;

        if (eventData.button == PointerEventData.InputButton.Right)
            borde.color = Color.blue;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        carta.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        carta.transform.localScale = scale;
    }
}
