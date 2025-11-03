using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject prefabEnemigo;
    public GameObject prefabPlayer;
    public GameObject prefabCarta;
    // UI
    public GameObject deathScreen;
    public GameObject victoryScreen;

    public static Boolean cartaSeleccionada;
    public static GameObject carta;
    public static GameObject player;
    public static Dictionary<GameObject, Vector2> enemigos = new();
    public static List<GameObject> enemigosLis = new();
    public static GameObject enemy; // De momento solo hay un enemigo
    public static List<Card> cardList;
    public static List<Enemy> enemyList;
    void Start()
    {
        cardList = new List<Card>(Resources.LoadAll<Card>("Cartas"));
        enemyList = new List<Enemy>(Resources.LoadAll<Enemy>("Enemigos"));
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
        InstanciateEnemy(new Vector2(2, 0), 1);
        InstanciateEnemy(new Vector2(2, 1), 1);
        InstanciateEnemy(new Vector2(2, 2), 1);
        InstanciateEnemy(new Vector2(3, 2), 1);
        TurnManager.playerController = player.GetComponent<PlayerController>();


    }
    public GameObject GetPrefabCarta() => prefabCarta;

    public void InstanciateEnemy(Vector2 pos, int id)
    {
        enemigosLis.Add(Instantiate(prefabEnemigo, GridManager._tiles[pos].transform.position, Quaternion.identity));
        enemigos.Add(enemigosLis[enemigosLis.Count - 1], pos);
        enemigosLis[enemigosLis.Count - 1].GetComponent<DisplayEnemy>().ActualizarID(id);
        GridManager._tiles[pos].ocupadoObj = enemigosLis[enemigosLis.Count - 1];
        GridManager._tiles[pos].ocupado = true;
    }
    
    public void TilesEnemigos()
    {
        foreach (var enemigo in enemigos)
        {
            enemigo.Key.GetComponent<TileManagerEnemigo>().CalculoTiles(enemigo.Key);
        }
    }

}
