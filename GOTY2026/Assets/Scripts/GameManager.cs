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
    }
    public GameObject GetPrefabCarta() => prefabCarta;
    
}
