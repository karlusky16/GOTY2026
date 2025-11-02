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
    public static Dictionary<GameObject, Vector2> enemigos = new Dictionary<GameObject, Vector2>();
    public static List<GameObject> enemigosLis = new List<GameObject>();
    public static GameObject enemy; // De momento solo hay un enemigo
    public static List<Card> cardList;
    void Start()
    {
        cardList = new List<Card>(Resources.LoadAll<Card>("Cartas"));
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
        InstanciateEnemy(new Vector2(2, 0));
        InstanciateEnemy(new Vector2(2, 1));
        InstanciateEnemy(new Vector2(2, 2));
        InstanciateEnemy(new Vector2(3, 2));
        TurnManager.playerController = player.GetComponent<PlayerController>();
        

    }
    public GameObject GetPrefabCarta() => prefabCarta;
    
    public void InstanciateEnemy(Vector2 pos)
    {
        enemigosLis.Add(Instantiate(prefabEnemigo, GridManager._tiles[pos].transform.position, Quaternion.identity));
        enemigos.Add(Instantiate(prefabEnemigo, GridManager._tiles[pos].transform.position, Quaternion.identity), pos);
        GridManager._tiles[pos].ocupadoObj = enemigosLis[enemigosLis.Count-1];
        GridManager._tiles[pos].ocupado = true;
    }

}
