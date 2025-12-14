using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public List<int> cartas;
    public int longMano;
    public int vidaMaxima;
    public int vidaActual;
    public int energiaMaxima;
    public int manaMaxima;
    public int monedas;
    public Boolean inicializado;
    public PlayerStats()
    {
        inicializado = false;
        vidaMaxima = 20;
        vidaActual = 20;
        energiaMaxima = 4;
        manaMaxima = 4;
        monedas = 0;
        cartas = new List<int>();
        longMano = 6;
    }
}
