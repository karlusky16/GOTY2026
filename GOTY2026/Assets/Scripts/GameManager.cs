using System;
using Unity.VisualScripting;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject prefabEnemigo;
    public GameObject prefabPlayer;
    public GameObject prefabCarta;
    void Start()
    {
        // Si la instancia no existe, crea una y marca el objeto para no ser destruido.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        GridManager._tiles[new Vector2(1, 1)].ocupadoObj = Instantiate(prefabEnemigo, GridManager._tiles[new Vector2(1, 1)].transform.position, Quaternion.identity);
        GridManager._tiles[new Vector2(1, 1)].ocupado = true;
    }
    public GameObject GetPrefabCarta() => prefabCarta;
    
}
