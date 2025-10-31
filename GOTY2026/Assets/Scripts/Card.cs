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
    public int tipoCoste;
    public String descripcion;
    public String patron;
    public int rango;
    public int area;
    public int tipo;
    public int daño;
    public Card()
    {

    }
    public Card(int id, String _name, String patron, int coste, int tipoCoste, String descripcion, int rango,
    int area, int tipo, int daño)
    {
        this.id = id;
        this._name = _name;
        this.coste = coste;
        this.tipoCoste = tipoCoste;
        this.patron = patron;
        this.descripcion = descripcion;
        this.rango = rango;
        this.area = area;
        this.tipo = tipo; // 0 ataque, 1 movimiento, 2 ataque movimiento, 3 boosteo
        this.daño = daño;
    }
    public Card(int id, String _name, String patron, int coste, int tipoCoste, String descripcion, int rango, int area, int tipo)
    {
        this.id = id;
        this._name = _name;
        this.coste = coste;
        this.tipoCoste = tipoCoste; // 0 es maná, 1 es energía y 2 es las dos
        this.patron = patron;
        this.descripcion = descripcion;
        this.rango = rango;
        this.area = area;
        this.tipo = tipo; // 0 ataque, 1 movimiento, 2 ataque movimiento, 3 boosteo
    }
}
