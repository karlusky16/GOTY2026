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
    public static Boolean cartaSeleccionada;
    public static GameObject carta;
    public static GameObject player;
    public static GameObject enemy; // De momento solo hay un enemigo
    public static List<Card> cardList = new();
    void Start()
    {
        cardList = new List<Card>(Resources.LoadAll<Card>("Cartas"));  
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(player);
        player.GetComponent<PlayerController>().Mover(new Vector2(0, 2));
        GridManager._tiles[new Vector2(4, 2)].ocupadoObj = Instantiate(prefabEnemigo, GridManager._tiles[new Vector2(4, 2)].transform.position, Quaternion.identity);
        GridManager._tiles[new Vector2(4, 2)].ocupado = true;
        TurnManager.playerController = player.GetComponent<PlayerController>();
        enemy = GameObject.FindWithTag("Enemy"); //De momento solo hay uno, cuando haya más igual hay que cambiarlo
    }
    public GameObject GetPrefabCarta() => prefabCarta;
    
}
