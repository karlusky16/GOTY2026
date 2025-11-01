using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static List<int> cartas = new();
    public static List<int> descartes = new();
    public Tile posicion;
    public static int longMano = 5;
    public Action<int> JugadorReduceVida;
    public Action<int> JugadorAumentaVida;

    public Action<int> JugadorReduceEnergia;
    public Action<int> JugadorAumentaEnergia;

    public Action<int> JugadorReduceMana;
    public Action<int> JugadorAumentaMana;

    [SerializeField] private int vidaMaxima;
    [SerializeField] private int vidaActual;

    [SerializeField] private int energiaMaxima;
    [SerializeField] private int energiaActual;

    [SerializeField] private int manaMaxima;
    [SerializeField] private int manaActual;

    private void Awake()
    {
        vidaActual = vidaMaxima;
        energiaActual = energiaMaxima;
        manaActual = manaMaxima;
    }
    public void Mover(UnityEngine.Vector2 pos)
    {
        posicion = GridManager._tiles[pos];
        gameObject.transform.position = posicion.transform.position;  
    }
    //Getters
    public Tile GetPos() => posicion;
    public int GetCartasLength() => cartas.Count;
    public int GetLongMano() => longMano;
    public List<int> GetCartas() => cartas;
    public int GetVidaMaxima() => vidaMaxima;
    public int GetVidaActual() => vidaActual;
    public int GetEnergiaMaxima() => energiaMaxima;
    public int GetEnergiaActual() => energiaActual;
    public int GetManaMaxima() => manaMaxima;
    public int GetManaActual() => manaActual;

    //Modificar vida del jugador
    public void ReducirVida(int vida)
    {
        if ((vidaActual -= vida) < 0) vidaActual = 0;
        JugadorReduceVida?.Invoke(vidaActual);
        Debug.Log("Reduce vida jugador");
    }
    public void AumentarVida(int vida)
    {
        if ((vidaActual += vida) > vidaMaxima) vidaActual = vidaMaxima;
        JugadorAumentaVida?.Invoke(vidaActual);
        Debug.Log("Aumenta vida jugador");
    }

    //Modificar energia del jugador
    public void ReducirEnergia(int energia)
    {
        if ((energiaActual -= energia) < 0) energiaActual = 0;
        JugadorReduceEnergia?.Invoke(energiaActual);
        Debug.Log("Reduce energia jugador");
    }
    public void AumentarEnergia(int energia)
    {
        if ((energiaActual += energia) > energiaMaxima) energiaActual = energiaMaxima;
        JugadorAumentaEnergia?.Invoke(energiaActual);
        Debug.Log("Aumenta energia jugador");
    }

    //Modificar mana del jugador
    public void ReducirMana(int mana)
    {
        if ((manaActual -= mana) < 0) manaActual = 0;
        JugadorReduceMana?.Invoke(manaActual);
        Debug.Log("Reduce mana jugador");
    }
    public void AumentarMana(int mana)
    {
        if ((manaActual += mana) > manaMaxima) manaActual = manaMaxima;
        JugadorAumentaMana?.Invoke(manaActual);
        Debug.Log("Aumenta mana jugador");
    }
    public void AddCarta(int id)
    {
        Debug.Log("AddCarta " + id);
        cartas.Add(id);
    }
    public void AddCartaDescartes(int id)
    {
        Debug.Log("AddCartaDescartes: " + id);
        descartes.Add(id);
    }
    public void DescartesABaraja()
    {
        Debug.Log("DescartesABaraja");
        cartas = new List<int>(descartes); 
        Debug.Log("Cartas en baraja: " + cartas.ToString());
        descartes.Clear();  
    }
}