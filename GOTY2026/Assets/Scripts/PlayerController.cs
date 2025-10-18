using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<int> JugadorRecibeDaño;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Getters
    public int GetVidaMaxima() => vidaMaxima;
    public int GetVidaActual() => vidaActual;

    //Modificar vida del jugador
    public void ReducirVida(int vida)
    {
        if ((vidaActual -= vida) < 0) vidaActual = 0;
        JugadorRecibeDaño?.Invoke(vidaActual);
    }
    public void AumentarVida(int vida)
    {
        if ((vidaActual += vida) > vidaMaxima) vidaActual = vidaMaxima;
    }

    //Modificar energia del jugador
    public void ReducirEnergia(int energia)
    {
        if ((energiaActual -= energia) < 0) energiaActual = 0;
    }
    public void AumentarEnergia(int energia)
    {
        if ((energiaActual += energia) > energiaMaxima) energiaActual = energiaMaxima;
    }

    //Modificar mana del jugador
    public void ReducirMana(int mana)
    {
        if ((manaActual -= mana) < 0) manaActual = 0;
    }
    public void AumentarMana(int mana)
    {
        if ((manaActual += mana) > manaMaxima) manaActual = manaMaxima;
    }
}
