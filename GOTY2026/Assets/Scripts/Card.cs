using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;
using System;
using UnityEngine.UI;
[System.Serializable]
public class Card
{
    public int id;
    public String _name;
    public int coste;
    public int tipo; // 0 para maná, 1 para energía  y 2 para los dos
    public String descripcion;
    public String patron;
    public Sprite sprite;
    public Card()
    {

    }
    //Constructor carta ataque
    public Card(int id, String _name, String patron, int coste, int tipo, String descripcion,
    Sprite sprite)
    {
        this.id = id;
        this._name = _name;
        this.tipo = tipo;
        this.coste = coste;
        this.patron = patron;
        this.descripcion = descripcion;
        this.sprite = sprite;
    }
}
