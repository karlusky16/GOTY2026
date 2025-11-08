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
    public static TextMeshProUGUI tileOcupada;
    public static GameObject interfaz;


    public bool pulsado = false;

    //De momento esto es asi ya que solo hay un enemigo

    public static PlayerController playerController;



    void Start()
    {
        ManejoBaraja.Inicializar();
        noMas = GameObject.Find("InterfazUsuario/NoMas").GetComponent<TextMeshProUGUI>();
        tileOcupada = GameObject.Find("InterfazUsuario/TileOcupada").GetComponent<TextMeshProUGUI>();
        interfaz = GameObject.Find("InterfazUsuario/NextTurn");
        noMas.gameObject.SetActive(false);
        tileOcupada.gameObject.SetActive(false);
        ManejoBaraja.ManoTurno();
        GameObject.Find("GameManager").GetComponent<GameManager>().TilesEnemigos();
        GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
        Debug.Log("Comienza el combate. Turno del jugador.");
    }

    void Update()

    {
        if (currentTurn == Turn.Player)
        {
            // Pongo espacio pero realmente sería seleccionar la carta y luego al enemigo o la habilidad de la carta
            //if (Input.GetKeyDown(KeyCode.Space) || pulsado == true)
            if (pulsado)
            {
                pulsado = false;
                //Aquí podriamos poner una llamada a un método para todo el proceso de seleccionar la carta y tal
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
        interfaz.SetActive(false);

    }

    void EndPlayerTurn()
    {
        if (GameManager.cartaSeleccionada == true)
        {
            GameObject.Find("GameManager").gameObject.SendMessage("DesmarcarRango", GameManager.player.GetComponent<PlayerController>().GetPos());
        }
        GameManager.carta = null;
        GameManager.cartaSeleccionada = false;
        if (CardAction.carta != null) Destroy(CardAction.carta);
        currentTurn = Turn.Enemy;
        ManejoBaraja.DevolverMano();
        Debug.Log("Turno del enemigo.");
        GameManager.CambiarLayerEnemy("Default");

    }

    void EnemyTurn()
    {
        // Aquí iría el ataque/movimiento del enemigo

        foreach (var enemy in GameManager.enemigosLis)
        {
            Debug.Log("Ataca el enemigo en: " + GameManager.enemigos[enemy]);
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
                enemy.GetComponent<EnemyController>().Ataque(enemy.GetComponent<TileManagerEnemigo>().GetRango(), enemy.GetComponent<DisplayEnemy>().GetDaño());
                Debug.Log("El enemigo ataca");

            }
        }
        Invoke("EndEnemyTurn", 1.5f);
        currentTurn = Turn.Player; //evita que ataque en cada frame
        playerController.AumentarEnergia(playerController.GetEnergiaMaxima());
        playerController.AumentarMana(playerController.GetManaMaxima());
    }

    void EndEnemyTurn()
    {
        currentTurn = Turn.Player;
        ManejoBaraja.ManoTurno();
        GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
        GameObject.Find("GameManager").GetComponent<GameManager>().TilesEnemigos();
        GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
        Debug.Log("Vuelve el turno del jugador.");
    }
    
    public static void ResetTurn()
    {
        currentTurn = Turn.Player;
    }
    public void ActivarAvanzar()
    {
        interfaz.SetActive(true);
    }
    
}
