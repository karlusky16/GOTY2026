using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    //Getters
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
}