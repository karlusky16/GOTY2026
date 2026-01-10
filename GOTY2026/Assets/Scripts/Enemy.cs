using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
public class Enemy : ScriptableObject
{
    public int id;
    public String _name;
    public int vida;
    public int daño;
    public int dañoFuego;
    public int shockValue;
    public int rango;
    public int area;
    public int movimiento;
    public String patronAtaque;
    public Sprite sprite;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    
}

/*
Aqui van los patrones de pueda tener cada enemigo
*/
