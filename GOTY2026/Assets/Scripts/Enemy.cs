using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
public class Enemy : ScriptableObject 
{
    public int id;
    public String _name;
    public int vida;
    public int daño;
    public int rango;
    public int area;
    public String patronAtaque;
    public Sprite sprite;
}

/*
Aqui van los patrones de pueda tener cada enemigo
*/
