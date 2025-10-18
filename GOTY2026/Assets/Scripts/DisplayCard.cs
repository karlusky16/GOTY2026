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
    public Sprite spriteImage;
    public String descripcion;
    public int tipo;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    public String patron;
    public Image imageArt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        card = CardDataBase.cardList[displayID - 1];
        this.id = card.id;
        this._name = card._name;
        this.coste = card.coste;
        this.descripcion = card.descripcion;
        this.patron = card.patron;
        this.spriteImage = card.sprite;
        this.tipo = card.tipo;

        nameText.text = " " + _name;
        costText.text = " " + coste;
        descriptionText.text = " " + descripcion;
        imageArt.sprite = spriteImage;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
