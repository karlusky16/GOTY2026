using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public enum Turn { Player, Enemy }
    public static int numTurno;
    public static Turn currentTurn = Turn.Player;
    public static TextMeshProUGUI noMas;
    public static TextMeshProUGUI tileOcupada;
    public static GameObject interfaz;
    public static List<GameObject> robo = new();
    public static List<GameObject> descartes = new();
    public GameObject prefabCarta; // tu prefab de carta
    public static GameObject _image; //referencia al CardPanel
    public bool pulsado = false;
    public GameObject descartesPadre, roboPadre;
    //De momento esto es asi ya que solo hay un enemigo

    public static PlayerController playerController;



    void Start()
    {
        numTurno = 0;
        ManejoBaraja.Inicializar();
        robo = new();
        descartes = new();
        InstanciarCartas();
        noMas = GameObject.Find("InterfazUsuario/NoMas").GetComponent<TextMeshProUGUI>();
        tileOcupada = GameObject.Find("InterfazUsuario/TileOcupada").GetComponent<TextMeshProUGUI>();
        interfaz = GameObject.Find("InterfazUsuario/NextTurn");
        noMas.gameObject.SetActive(false);
        tileOcupada.gameObject.SetActive(false);
        GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().ManoTurno();
        GameObject.Find("GameManager").GetComponent<GameManager>().TilesEnemigos();
        //GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
        GameObject.Find("Player").GetComponent<PlayerController>().ResetMirilla();
        playerController.AumentarEnergia(playerController.GetEnergiaMaxima());
        playerController.AumentarMana(playerController.GetManaMaxima());
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

    public void cargarEscena()
    {
        SceneManager.LoadScene("Recompensas");
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
        GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().DevolverMano(); ;
        Debug.Log("Turno del enemigo.");
        GameManager.CambiarLayerEnemy("Default");

    }

    void EnemyTurn()
    {
        foreach (var enemy in GameManager.enemigosLis)
        {
            enemy.GetComponent<BoxCollider2D>().enabled = false; 
        }
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
                if (enemy.GetComponent<DisplayEnemy>().enemy.id == 4)
                {
                    GameObject.Find("Player").GetComponent<PlayerController>().ReducirVida(enemy.GetComponent<DisplayEnemy>().GetDaño());
                }
                else
                {
                    Debug.Log("Rango enemigo: " + enemy.GetComponent<TileManagerEnemigo>().GetRango());
                    enemy.GetComponent<EnemyController>().Ataque(enemy.GetComponent<TileManagerEnemigo>().GetRango(), enemy.GetComponent<DisplayEnemy>().GetDaño());
                }
                enemy.GetComponent<EnemyController>().Fuego();
                enemy.GetComponent<EnemyController>().Movimiento(enemy);
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
        GameObject.Find("Player").GetComponent<PlayerController>().ResetMirilla();
        numTurno += 1;
        currentTurn = Turn.Player;
        GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().ManoTurno();
        GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
        GameObject.Find("GameManager").GetComponent<GameManager>().TilesEnemigos();
        //GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
        Debug.Log("Vuelve el turno del jugador.");
        foreach (var enemy in GameManager.enemigosLis)
        {
            enemy.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public static void ResetTurn()
    {
        currentTurn = Turn.Player;
    }
    public void ActivarAvanzar()
    {
        interfaz.SetActive(true);
    }

    void InstanciarCartas()
    {
        System.Random rand = new();
        int cartas = GameManager.player.GetComponent<PlayerController>().GetCartasLength();
        List<int> cardList = GameManager.player.GetComponent<PlayerController>().GetCartas();
        for (int i = 0; i < cartas; i++)
        {
            robo.Add(Instantiate(prefabCarta, roboPadre.transform));
            robo[i].GetComponent<DisplayCard>().ActualizarID(cardList[i]);
        }
    }

}
