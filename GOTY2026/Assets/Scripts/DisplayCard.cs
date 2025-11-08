using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    public Card card;
    public int displayID;
    public int id;
    public String _name;
    public int coste;
    public int tipo;
    public String descripcion;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    public String patron;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
<<<<<<< Updated upstream
        card = CardDataBase.cardList[displayID - 1];
        this.id = card.id;
        this._name = card._name;
        this.coste = card.coste;
        this.descripcion = card.descripcion;
        nameText.text = " " + _name;
        costText.text = " " + coste;
        descriptionText.text = " " + descripcion;
=======
        Debug.Log("ActualizarID llamado con ID: " + (displayID - 1));
        Debug.Log("CardList Count: " + GameManager.cardList.Count);
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
>>>>>>> Stashed changes
    }
    public Card GetCard() => card;
    public int GetTipo() => card.tipo;
    public String GetPatron() => card.patron;
    public int GetRango() => card.rango;
    public int GetArea() => card.area;
    // Update is called once per frame
}
