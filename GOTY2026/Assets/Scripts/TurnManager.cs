using System;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public enum Turn { Player, Enemy }
    public static Turn currentTurn = Turn.Player;
    public static TextMeshProUGUI noMas;
    public static GameObject botonNextTurn;
    

    public bool pulsado = false;

    //De momento esto es asi ya que solo hay un enemigo
    public static GameObject enemy;
    
    

    void Start()
    {
        ManejoBaraja.Inicializar();
        noMas = GameObject.Find("InterfazUsuario/NoMas").GetComponent<TextMeshProUGUI>();
        noMas.gameObject.SetActive(false);
        ManejoBaraja.ManoTurno();
        Debug.Log("Comienza el combate. Turno del jugador.");

        enemy = GameManager.enemy;
    }

    void Update()
   
    {
        if (currentTurn == Turn.Player)
        {
            // Pongo espacio pero realmente sería seleccionar la carta y luego al enemigo o la habilidad de la carta
            if (Input.GetKeyDown(KeyCode.Space) || pulsado == true)
            {
                pulsado = false;
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
    

    public void pulsaBotonAvanzar()
    {
        pulsado = true;
    }

    void EndPlayerTurn()
    {

        GameManager.carta = null;
        GameManager.cartaSeleccionada = false;
        if (CardAction.carta != null) Destroy(CardAction.carta);
        currentTurn = Turn.Enemy;
        ManejoBaraja.DevolverMano();
        Debug.Log("Turno del enemigo.");
    }

    void EnemyTurn()
    {
        // Aquí iría el ataque/movimiento del enemigo
        if (enemy == null)
        {
            enemy = GameManager.enemy;
        }
        if (enemy == null)
        {
            Debug.Log("El enemigo no ha podido atacar porque enemy sigue siendo null");

        }
        else
        {
            if (enemy.GetComponent<EnemyController>() == null)
            {
                //Debug.Log("El enemigo ataca");
                //enemy.GetComponent<EnemyController>().Ataque();
                Debug.Log("enemy.GetComponent<EnemyController>() == null");

                // Espera
                Invoke("EndEnemyTurn", 1.5f);

                currentTurn = Turn.Player; //evita que ataque en cada frame
            }
            else
            {
                enemy.GetComponent<EnemyController>().Ataque();
                Debug.Log("El enemigo ataca");
                Invoke("EndEnemyTurn", 1.5f);

                currentTurn = Turn.Player; //evita que ataque en cada frame
            }
        }
    }

    void EndEnemyTurn()
    {
        currentTurn = Turn.Player;
        ManejoBaraja.ManoTurno();
        Debug.Log("Vuelve el turno del jugador.");
    }
    
}
