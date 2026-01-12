using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBoss : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject victoryScreen;
    GameManager GameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.enemigos.Clear();
        GameManager.obstacles.Clear();
        GameManager.enemigosLis.Clear();
        GameManager.obstaclesLis.Clear();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameManager.player.GetComponent<PlayerController>().Mover(new Vector2(0, 2));
        GenerarEnemigos();
        TurnManager.playerController = GameManager.player.GetComponent<PlayerController>();
        GenerarObstaculos();
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }


    public void GenerarEnemigos()
    {
        GameManager.InstanciateEnemy(new Vector2(8, 2), 10);
    }

    public void GenerarObstaculos()
    {
        GameManager.InstanciateObstacle(new Vector2(8, 0), 0);
        GameManager.InstanciateObstacle(new Vector2(8, 1), 0);
        GameManager.InstanciateObstacle(new Vector2(8, 3), 0);
        GameManager.InstanciateObstacle(new Vector2(8, 4), 0);
        GameManager.InstanciateObstacle(new Vector2(3, 1), 5);
        GameManager.InstanciateObstacle(new Vector2(1, 0), 5);
        GameManager.InstanciateObstacle(new Vector2(2, 0), 5);
        GameManager.InstanciateObstacle(new Vector2(2, 1), 5);
        GameManager.InstanciateObstacle(new Vector2(3, 0), 5);
        GameManager.InstanciateObstacle(new Vector2(2, 4), 4);
        GameManager.InstanciateObstacle(new Vector2(3, 4), 4);
        GameManager.InstanciateObstacle(new Vector2(4, 4), 4);
        GameManager.InstanciateObstacle(new Vector2(4, 3), 4);
        GameManager.InstanciateObstacle(new Vector2(5, 4), 4);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void LoadGameOver()
    {
        deathScreen.SetActive(true);
    }
    public void Reset()
    {
        GameManager.reset = true;
        Destroy(GameManager.player);
        Destroy(GameObject.Find("GameManager").GetComponent<GameManager>());
        if (File.Exists( Application.persistentDataPath + "/save.json"))
        {
            File.Delete( Application.persistentDataPath + "/save.json");
            Debug.Log("Archivo de guardado eliminado");
        }
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void Salir()
    {
        #if UNITY_EDITOR
        // Si estás en el editor, detiene el Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Si estás en un build (EXE, Mac, etc.), cierra la aplicación
        Application.Quit();
        #endif
    }
    public void LoadVictory()
    {
        victoryScreen.SetActive(true);
        
    }
}
