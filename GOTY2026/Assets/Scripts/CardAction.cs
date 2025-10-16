using System;
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
        if (eventData.button == PointerEventData.InputButton.Left && !Player.cartaSeleccionada)
        {
            Player.cartaSeleccionada = true;
            borde.color = Color.red;
            Player.carta = gameObject;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Player.cartaSeleccionada = false;
            borde.color = Color.blue;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Player.cartaSeleccionada) {
           carta.transform.localScale = new Vector3(1f, 1f, 1f);
           carta.transform.position = new Vector3(posicion.x, posicion.y + 75f, posicion.z);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        carta.transform.localScale = scale;
        carta.transform.position = posicion;
    }
}

