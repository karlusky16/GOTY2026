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
    public TextMeshProUGUI descriptionText;
    public String patron;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        card = CardDataBase.cardList[displayID - 1];
        nameText.text = " " + card._name;
        costText.text = " " + card.coste;
        descriptionText.text = " " + card.descripcion;
    }
    public Card GetCard() => card;
    public int GetTipo() => card.tipo;
    public int GetDaño() => card.daño;
    public int GetTipoCoste() => card.tipoCoste;
    public int GetCoste() => card.coste;
    public String GetPatron() => card.patron;
    public int GetRango() => card.rango;
    public int GetArea() => card.area;
    // Update is called once per frame
}
