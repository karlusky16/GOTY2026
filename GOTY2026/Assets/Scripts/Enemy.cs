using System;
using UnityEngine;

public class Enemy
{
    public int id;
    public String _name;
    public int vida;
    public int daño;
    public EnemyPattern patronAtaque;
    public Enemy()
    {

    }
    public Enemy(int id, String _name, int vida, int daño, EnemyPattern patronAtaque)
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
public enum EnemyPattern
{
    Cruz
}
