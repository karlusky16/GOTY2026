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
    public static List<GameObject> enemigos = new List<GameObject>();
    public static GameObject enemy; // De momento solo hay un enemigo
    void Start()
    {
        // Si la instancia no existe, crea una y marca el objeto para no ser destruido.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            player = GameObject.FindWithTag("Player");
            DontDestroyOnLoad(player);
            player.GetComponent<PlayerController>().Mover(new Vector2 (0,2));
        }
        //añade enemigo al array de enemigos.
        enemigos.Add(Instantiate(prefabEnemigo, GridManager._tiles[new Vector2(4, 2)].transform.position, Quaternion.identity));
        GridManager._tiles[new Vector2(4, 2)].ocupadoObj = enemigos[0];
        GridManager._tiles[new Vector2(4, 2)].ocupado = true;
        TurnManager.playerController = player.GetComponent<PlayerController>();
       

    }
    public GameObject GetPrefabCarta() => prefabCarta;
    
}
