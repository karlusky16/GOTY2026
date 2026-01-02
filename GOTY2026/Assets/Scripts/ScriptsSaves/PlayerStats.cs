using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public List<int> cartas;
    public List<int> pasivos;
    public int longMano;
    public int vidaMaxima;
    public int vidaActual;
    public int energiaMaxima;
    public int manaMaxima;
    public int monedas;
    public Boolean inicializado;
    public int dañoItems;
    public int escudoItems;
    public int danoFuegoItems;
    public int valorAturdidoItems;
    public PlayerStats()
    {
        inicializado = false;
        vidaMaxima = 20;
        vidaActual = 20;
        energiaMaxima = 4;
        manaMaxima = 4;
        monedas = 0;
        cartas = new List<int>();
        pasivos = new List<int>();
        longMano = 6;
        dañoItems = 0;
        escudoItems = 0;
        danoFuegoItems = 0;
        valorAturdidoItems = 0;
    }
}
