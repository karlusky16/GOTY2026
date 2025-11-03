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
    public Enemy()
    {

    }
    public Enemy(int id, String _name, int vida, int daño, String patronAtaque)
    {
        this.id = id;
        this._name = _name;
        this.vida = vida;
        this.daño = daño;
        this.patronAtaque = patronAtaque;
    }
}

/*
Aqui van los patrones de pueda tener cada enemigo
*/
