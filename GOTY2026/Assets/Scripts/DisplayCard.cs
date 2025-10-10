using System;
using System.Collections.Generic;
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
    public Text nameText;
    public Text costText;
    public Text descriptionText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        card = CardDataBase.cardList[displayID];
    }

    // Update is called once per frame
    void Update()
    {
        this.id = card.id;
        this._name = card._name;
        this.coste = card.coste;
        this.tipo = card.tipo;
        this.descripcion = card.descripcion;
        nameText.text = " " + _name;
        costText.text = " " + coste;
        descriptionText.text = " " + descripcion;

    }
}
