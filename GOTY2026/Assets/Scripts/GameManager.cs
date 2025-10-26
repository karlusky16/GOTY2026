using System;
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
        GridManager._tiles[new Vector2(1, 1)].ocupadoObj = Instantiate(prefabEnemigo, GridManager._tiles[new Vector2(1, 1)].transform.position, Quaternion.identity);
        GridManager._tiles[new Vector2(1, 1)].ocupado = true;

        enemy = GameObject.FindWithTag("Enemy"); //De momento solo hay uno, cuando haya más igual hay que cambiarlo

    }
    public GameObject GetPrefabCarta() => prefabCarta;
    
}
