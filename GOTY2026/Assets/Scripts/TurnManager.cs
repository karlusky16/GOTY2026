using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool viendoTE = false;
    public GameObject descartesPadre, roboPadre;
    //De momento esto es asi ya que solo hay un enemigo

    public static PlayerController playerController;
    private Animator animator;

    void Start()
    {
        numTurno = 0;
        ManejoBaraja.Inicializar();
        robo = new();
        descartes = new();
        InstanciarCartas();
        noMas = GameObject.Find("InterfazUsuario/NoMas").GetComponent<TextMeshProUGUI>();
        interfaz = GameObject.Find("InterfazUsuario/NextTurn");
        GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().ManoTurno();
        GameObject.Find("GameManager").GetComponent<GameManager>().TilesEnemigos();
        playerController.QuitarShock();
        playerController.ResetMirilla();
        playerController.ResetTemporales();
        playerController.AumentarEnergia(playerController.GetEnergiaMaxima());
        playerController.AumentarMana(playerController.GetManaMaxima());
        playerController.fuego.gameObject.SetActive(false);
        playerController.aturdido.gameObject.SetActive(false);
        playerController.escudoS.gameObject.SetActive(false);
        playerController.AddEscudo(playerController.escudoItems);
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
            currentTurn = Turn.Player; // evita repetirse
            StartCoroutine(EnemyTurn());
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
            GameObject.Find("GameManager").SendMessage("DesmarcarRango", GameManager.player.GetComponent<PlayerController>().GetPos());
        }
        if (viendoTE)
        {
            VerTilesEnemigos();
            viendoTE = false;
        }
        GameManager.carta = null;
        GameManager.cartaSeleccionada = false;
        playerController.QuitarShock();
        if (CardAction.carta != null) Destroy(CardAction.carta);
        currentTurn = Turn.Enemy;
        GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().DevolverMano(); ;
        Debug.Log("Turno del enemigo.");
        GameManager.CambiarLayerEnemy("Default");

    }

    IEnumerator EnemyTurn()
    {
        // 1️⃣ Obstáculos (se hace instantáneo)
        foreach (var obstacle in GameManager.obstacles.Keys.ToList())
        {
            var d = obstacle.GetComponent<DisplayObstacle>();
            d.turnosRestantes--;

            if (d.turnosRestantes <= 0 && d.obstacle.atravesable)
            {
                GameManager.obstacles.TryGetValue(obstacle, out Vector2 pos);
                GridManager._tiles[pos].ocupadoAt = false;
                GridManager._tiles[pos].ocupadoObjAt = null;
                GameManager.obstacles.Remove(obstacle);
                GameManager.obstaclesLis.Remove(obstacle);
                Destroy(obstacle);
            }
        }

        // 2️⃣ Enemigos UNO A UNO
        foreach (var enemy in GameManager.enemigosLis)
        {
            Debug.Log("Ataca el enemigo en: " + GameManager.enemigos[enemy]);

            var animator = enemy.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("ataque", true);
                if (enemy.GetComponent<DisplayEnemy>().GetEnemy().id == 9)
                {
                    animator.SetInteger("turno", numTurno+1);
                    
                }
            }

            yield return new WaitForSeconds(1f);
            enemy.GetComponent<EnemyController>().HacerObstaculos(enemy.GetComponent<TileManagerEnemigo>().GetRango());
            var display = enemy.GetComponent<DisplayEnemy>();
            var controller = enemy.GetComponent<EnemyController>();

            if (display.enemy.id == 4 ||
                (display.enemy.id == 10 &&
                enemy.GetComponent<TileManagerEnemigo>().patronDragon == 5))
            {
                playerController.ReducirVida(display.GetDaño());
                playerController.AddFuego(display.enemy.dañoFuego);
                playerController.AddShock(display.enemy.shockValue);
            }
            else
            {
                controller.Ataque(
                    enemy.GetComponent<TileManagerEnemigo>().GetRango(),
                    display.GetDaño(),
                    display.enemy.dañoFuego,
                    display.enemy.shockValue
                );
            }

            controller.Movimiento(enemy);
            controller.Fuego();
            controller.ResetShock();

            enemy.GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
        EndEnemyTurn();
    }

    void EndEnemyTurn()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().ResetMirilla();
        numTurno += 1;
        currentTurn = Turn.Player;
        GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().ManoTurno();
        GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
        GameObject.Find("GameManager").GetComponent<GameManager>().TilesEnemigos();
        GameObject.Find("Player").GetComponent<PlayerController>().AumentarMana(GameObject.Find("Player").GetComponent<PlayerController>().GetManaMaxima());
        GameObject.Find("Player").GetComponent<PlayerController>().AumentarEnergia(GameObject.Find("Player").GetComponent<PlayerController>().GetEnergiaMaxima());
        GameObject.Find("Player").GetComponent<PlayerController>().ResetTemporales();
        playerController.AddEscudo(playerController.escudoItems);
        Debug.Log("Vuelve el turno del jugador.");
        foreach (var enemy in GameManager.enemigosLis)
        {
            enemy.GetComponent<BoxCollider2D>().enabled = true;
        }
        playerController.Fuego();
        playerController.RedEscudo(playerController.escudo - playerController.escudoItems);
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

    public void VerTilesEnemigos()
    {
        if (!viendoTE)
        {
            viendoTE = true;
            foreach (var enemigo in GameManager.enemigosLis)
            {
                enemigo.GetComponent<TileManagerEnemigo>().HighlightEnemyTiles(enemigo);
            }
        }
        else
        {
            viendoTE = false;
            foreach (var enemigo in GameManager.enemigosLis)
            {
                enemigo.GetComponent<TileManagerEnemigo>().UnHighlightEnemyTiles();
            }
        }

    }
}
