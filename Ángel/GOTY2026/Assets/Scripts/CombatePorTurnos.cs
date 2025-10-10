using System;
using UnityEngine;
using UnityEngine.UI;
public class CombatePorTurnos : MonoBehaviour
{
    public GameObject UI;
    GameObject[] Cartas;
    public Button sig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Console.Write("Turno Jugador");
        sig.onClick.AddListener(TurnoEnemigos);
    }
    void TurnoJugador()
    {
        UI.SetActive(true);
        Console.Write("Turno Jugador");
    }
    void TurnoEnemigos()
    {
        Console.Write("Turno Enemigos");
        UI.SetActive(false);
        new WaitForSeconds(1);
        TurnoJugador();
    }
}
