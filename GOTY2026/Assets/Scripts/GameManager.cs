using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject prefabEnemigo;
    public GameObject prefabPlayer;
    public GameObject prefabCarta;

    public GameObject prefabObstaculo;
    // UI
    public GameObject deathScreen;
    public GameObject victoryScreen;

    public static Boolean cartaSeleccionada;
    public static GameObject carta;
    public static GameObject player;
    public static Dictionary<GameObject, Vector2> enemigos = new();
    public static Dictionary<GameObject, Vector2> obstacles = new();
    public static List<GameObject> enemigosLis = new();
    public static List<GameObject> obstaclesLis = new();
    public static GameObject enemy;
    public static GameObject obstacle;
    public static List<Card> cardList;
    public static List<Enemy> enemyList;

    public static List<Obstacle> obstacleList;

    
    void Start()
    {
        cardList = new List<Card>(Resources.LoadAll<Card>("Cartas"));
        enemyList = new List<Enemy>(Resources.LoadAll<Enemy>("Enemigos"));
        obstacleList = new List<Obstacle>(Resources.LoadAll<Obstacle>("Obstacles"));
        // Si la instancia no existe, crea una y marca el objeto para no ser destruido.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            player = GameObject.FindWithTag("Player");
            DontDestroyOnLoad(player);
            player.GetComponent<PlayerController>().Mover(new Vector2(0, 2));
        }
        //añade enemigo al array de enemigos.
        GenerarEnemigos();
        TurnManager.playerController = player.GetComponent<PlayerController>();
        GenerarObstaculos();


    }
    public GameObject GetPrefabCarta() => prefabCarta;

    public void InstanciateEnemy(Vector2 pos, int id)
    {
        enemigosLis.Add(Instantiate(prefabEnemigo, new(GridManager._tiles[pos].transform.position.x, GridManager._tiles[pos].transform.position.y, -0.01f), Quaternion.identity));
        enemigos.Add(enemigosLis[enemigosLis.Count - 1], pos);
        enemigosLis[enemigosLis.Count - 1].GetComponent<DisplayEnemy>().ActualizarID(id);
        GridManager._tiles[pos].ocupadoObj = enemigosLis[enemigosLis.Count - 1];
        GridManager._tiles[pos].ocupado = true;
    }

    public void InstanciateObstacle(Vector2 pos, int id)
    {
        if (GridManager._tiles[pos].ocupadoObj == null)
        {
            obstaclesLis.Add(Instantiate(prefabObstaculo, new(GridManager._tiles[pos].transform.position.x, GridManager._tiles[pos].transform.position.y, -0.01f), Quaternion.identity));
            obstacles.Add(obstaclesLis[obstaclesLis.Count - 1], pos);
            obstaclesLis[obstaclesLis.Count - 1].GetComponent<DisplayObstacle>().ActualizarID(id);
            GridManager._tiles[pos].ocupadoObj = prefabObstaculo;
            GridManager._tiles[pos].ocupado = true;
        }
        else
        {
            Debug.LogError(" La casilla donde se ha intentado instanciar el obstaculo no estaba vacía");
        }
    }

    public void TilesEnemigos()
    {
        foreach (var enemigo in enemigos)
        {
            enemigo.Key.GetComponent<TileManagerEnemigo>().CalculoTiles(enemigo.Key);
        }
    }

    public static void CambiarLayerEnemy(String layer)
    {
        foreach (GameObject enemy in GameManager.enemigosLis)
        {
            enemy.layer = LayerMask.NameToLayer(layer);
        }
    }
    
    public void ResetGame()
    {
        GridManager.ResetTablero();
        ManejoBaraja.ResetBaraja();
        foreach (var e in enemigos)
        {
            enemigosLis.Remove(e.Key);
            if (e.Key != null)
                Destroy(e.Key);
        }
        enemigos.Clear();
        GenerarEnemigos();
        player.GetComponent<PlayerController>().ResetPlayer();
        player.GetComponent<PlayerController>().Mover(new Vector2(0, 2));
        TurnManager.ResetTurn();
        victoryScreen.SetActive(false);
        deathScreen.SetActive(false);
    }
    public void GenerarEnemigos()
    {
        InstanciateEnemy(new Vector2(4, 4), 1);
        InstanciateEnemy(new Vector2(2, 4), 1);
        InstanciateEnemy(new Vector2(2, 2), 1);
        InstanciateEnemy(new Vector2(2, 1), 1);
    }
    
    public void GenerarObstaculos()
    {
        InstanciateObstacle(new Vector2(2, 3), 1);
        InstanciateObstacle(new Vector2(4, 3), 1);

    }
    public void Salir()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

}
