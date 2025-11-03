using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;
using System;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject
{
    public int id;
    public String _name;
    public int coste;
    public int tipoCoste;// 0 es maná, 1 es energía y 2 es las dos
    public String descripcion;
    public String patron;
    public int rango;
    public int area;
    public int area2;
    public int tipo;// 0 ataque, 1 movimiento, 2 ataque movimiento, 3 boosteo
    public int daño;
    public Sprite sprite;
}
