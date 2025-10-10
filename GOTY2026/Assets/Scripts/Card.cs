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
    public int tipo;
    public String descripcion;
    public Card()
    {

    }
    public Card(int id, String _name, int coste, int tipo, String descripcion)
    {
        this.id = id;
        this._name = _name;
        this.coste = coste;
        this.tipo = tipo;
        this.descripcion = descripcion;
    }
}
