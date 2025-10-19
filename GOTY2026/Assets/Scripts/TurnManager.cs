using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Turn { Player, Enemy }
    public Turn currentTurn = Turn.Player;

    public static Boolean cartaSeleccionada;
    public static GameObject carta;

    void Start()
    {
        /*noMas = GameObject.Find("InterfazJugador/Nomas").GetComponent<TextMeshProUGUI>();
        noMas.gameObject.SetActive(false);*/
        Debug.Log("Comienza el combate. Turno del jugador.");
    }

    void Update()
    {
        if (currentTurn == Turn.Player)
        {
            // Pongo espacio pero realmente sería seleccionar la carta y luego al enemigo o la habilidad de la carta
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Aquí podriamos poner una llamada a un método para todo el proceso de seleccionar la carta y tal
                Debug.Log("El jugador usa la carta");
                EndPlayerTurn();
            }
        }
        else if (currentTurn == Turn.Enemy)
        {
            EnemyTurn();
        }
    }

    void EndPlayerTurn()
    {
        // Aquí podrías poner animaciones o efectos
        currentTurn = Turn.Enemy;
        Debug.Log("Turno del enemigo.");
    }

    void EnemyTurn()
    {
        // Aquí iría el ataque/movimiento del enemigo
        Debug.Log("El enemigo ataca");

        // Espera
        Invoke("EndEnemyTurn", 1.5f);
        
        currentTurn = Turn.Player; //evita que ataque en cada frame
    }

    void EndEnemyTurn()
    {
        currentTurn = Turn.Player;
        Debug.Log("Vuelve el turno del jugador.");
    }
}
