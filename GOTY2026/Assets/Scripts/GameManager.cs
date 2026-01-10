using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject prefabEnemigo;
    public GameObject prefabPlayer;
    public GameObject prefabCarta;
    public GameObject prefabObstaculo;
    public static bool estamina;
    public static bool cartaSeleccionada;
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
    public static List<ItemPasivo> itemsLis;
    public static string[] combatSceneList = new string[] { "SampleScene", "Combate1", "Combate2", "Combate3", "CombateBoss" };
    public static int indexScene = 0;
    public static bool reset;

    public static List<Obstacle> obstacleList;


    void Start()
    {
        reset = true;
        cardList = new List<Card>(Resources.LoadAll<Card>("Cartas"));
        enemyList = new List<Enemy>(Resources.LoadAll<Enemy>("Enemigos"));
        obstacleList = new List<Obstacle>(Resources.LoadAll<Obstacle>("Obstacles"));
        itemsLis = new List<ItemPasivo>(Resources.LoadAll<ItemPasivo>("ItemsPasivos"));
        // Si la instancia no existe, crea una y marca el objeto para no ser destruido.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            player = GameObject.FindWithTag("Player");
            DontDestroyOnLoad(player);
            //
        }
        //añade enemigo al array de enemigos.
        


    }
    public GameObject GetPrefabCarta() => prefabCarta;

    public void InstanciateEnemy(Vector2 pos, int id)
    {
        enemigosLis.Add(Instantiate(prefabEnemigo, new(GridManager._tiles[pos].transform.position.x, GridManager._tiles[pos].transform.position.y, -0.1f), Quaternion.identity));
        enemigos.Add(enemigosLis[^1], pos);
        enemigosLis[^1].GetComponent<DisplayEnemy>().ActualizarID(id);
        GridManager._tiles[pos].ocupadoObj = enemigosLis[enemigosLis.Count - 1];
        enemigosLis[^1].GetComponent<EnemyController>().Mover(pos); // le asigna al enemigo su posición        
    }

    public void InstanciateObstacle(Vector2 pos, int id)
    {
        if (id > 1)
        {
            if (GridManager._tiles[pos].ocupadoObjAt == null)
            {
                obstaclesLis.Add(Instantiate(prefabObstaculo, new(GridManager._tiles[pos].transform.position.x, GridManager._tiles[pos].transform.position.y, -0.01f), Quaternion.identity));
                obstacles.Add(obstaclesLis[^1], pos);
                obstaclesLis[^1].GetComponent<DisplayObstacle>().ActualizarID(id);
                GridManager._tiles[pos].ocupadoObjAt = obstaclesLis[^1];
                GridManager._tiles[pos].ocupadoAt = true;
            }
            else
            {
                Debug.LogError(" La casilla donde se ha intentado instanciar el obstaculo no estaba vacía");
            }
        }
        else
        {
            if (GridManager._tiles[pos].ocupadoObj == null)
            {
                obstaclesLis.Add(Instantiate(prefabObstaculo, new(GridManager._tiles[pos].transform.position.x, GridManager._tiles[pos].transform.position.y, -0.01f), Quaternion.identity));
                obstacles.Add(obstaclesLis[^1], pos);
                obstaclesLis[^1].GetComponent<DisplayObstacle>().ActualizarID(id);
                GridManager._tiles[pos].ocupadoObj = obstaclesLis[^1];
                GridManager._tiles[pos].ocupado = true;
            }
            else
            {
                Debug.LogError(" La casilla donde se ha intentado instanciar el obstaculo no estaba vacía");
            }
        }
    }

    public void TilesEnemigos()
{
    Debug.Log($"TilesEnemigos: enemigos.Count = {enemigos.Count}");
    foreach (var kv in enemigos)
    {
        var go = kv.Key;
        if (go == null)
        {
            Debug.LogWarning("TilesEnemigos: encontrado key NULL en GameManager.enemigos");
            continue;
        }

        Debug.Log($"TilesEnemigos: key='{go.name}' instanceId={go.GetInstanceID()} active={go.activeSelf} scene='{go.scene.name}'");

        var tme = go.GetComponent<TileManagerEnemigo>();
        if (tme == null)
        {
            Debug.LogError($"TilesEnemigos: GameObject '{go.name}' en GameManager.enemigos NO tiene TileManagerEnemigo. Componentes:");
            foreach (var c in go.GetComponents<Component>())
            {
                Debug.Log($" - {c.GetType().Name}");
            }
        }
        else
        {
            Debug.Log($"TilesEnemigos: '{go.name}' tiene TileManagerEnemigo, llamando CalculoTiles");
            tme.CalculoTiles(go);
        }
    }
}

    public static void CambiarLayerEnemy(String layer)
    {
        foreach (GameObject enemy in GameManager.enemigosLis)
        {
            enemy.layer = LayerMask.NameToLayer(layer);
        }
        player.layer = LayerMask.NameToLayer(layer);
    }

    public void ResetGame()
    {
        GridManager.ResetTablero();
        //ManejoBaraja.ResetBaraja();
        foreach (var e in enemigos)
        {
            enemigosLis.Remove(e.Key);
            if (e.Key != null)
                Destroy(e.Key);
        }
        enemigos.Clear();
        player.GetComponent<PlayerController>().ResetPlayer();
        player.GetComponent<PlayerController>().Mover(new Vector2(0, 2));
        GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
        TilesEnemigos();
        GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
        TurnManager.ResetTurn();
        /*Destroy(GameManager.instance.gameObject);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);*/
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void VolverMapa()
    {
        
    }

}
