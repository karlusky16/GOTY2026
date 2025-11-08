using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Windows;

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
    public Image borde;
    public Color _color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        card = GameManager.cardList[displayID - 1];
        nameText.text = " " + card._name;
        costText.text = " " + card.coste;
        DañoText.text = "Daño " + card.daño + "\n" + "Rango " + card.rango + "\n";
        descriptionText.text = " " + card.descripcion;
        cardImage.sprite = card.sprite;
        Color32 mana = new(95, 16, 120, 255);
        Color32 energia = new(29, 115, 0,255);
        if (GetTipoCoste() == 0)
        {
            borde.color = mana;
            _color = mana;
        }
        else if (GetTipoCoste() == 1)
        {
            borde.color = energia;
            _color = energia;
        }
        else if (GetTipoCoste() == 2)
        {
            borde.color =  Color.Lerp(mana, energia, 0.5f);
            _color = borde.color;
        }
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
    public Color GetColor() => _color;
    // Update is called once per frame
}
