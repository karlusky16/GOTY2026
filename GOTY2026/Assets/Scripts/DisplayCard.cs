using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    public Card card;
    public int displayID;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI DañoText;
    public TextMeshProUGUI descriptionText;
    public String patron;
    public Image cardImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        Debug.Log("ActualizarID llamado con ID: " + (displayID - 1));
        Debug.Log("CardList Count: " + GameManager.cardList.Count);
        card = GameManager.cardList[displayID - 1];
        nameText.text = " " + card._name;
        costText.text = " " + card.coste;
        DañoText.text = "Daño " + card.daño + "\n" + "Rango" + card.rango + "\n";
        descriptionText.text = " " + card.descripcion;
        cardImage.sprite = card.sprite;
    }
    public Card GetCard() => card;
    public int GetTipo() => card.tipo;
    public int GetDaño() => card.daño;
    public int GetTipoCoste() => card.tipoCoste;
    public int GetCoste() => card.coste;
    public String GetPatron() => card.patron;
    public int GetRango() => card.rango;
    public int GetArea() => card.area;
    public int GetArea2() => card.area2;
    // Update is called once per frame
}
