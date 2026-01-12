using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager4 : MonoBehaviour
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
        //GenerarObstaculos();
        deathScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }


    public void GenerarEnemigos()
    {
        GameManager.InstanciateEnemy(new Vector2(8, 1), 9);
        GameManager.InstanciateEnemy(new Vector2(7, 0), 5);
        GameManager.InstanciateEnemy(new Vector2(6, 2), 7);
        GameManager.InstanciateEnemy(new Vector2(7, 3), 8);
    }

    public void GenerarObstaculos()
    {
        GameManager.InstanciateObstacle(new Vector2(0, 4), 5);
        GameManager.InstanciateObstacle(new Vector2(0, 3), 5);
        GameManager.InstanciateObstacle(new Vector2(1, 4), 5);
        GameManager.InstanciateObstacle(new Vector2(2, 5), 5);
        GameManager.InstanciateObstacle(new Vector2(4, 0), 4);
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
        Destroy(GameManager);
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
}
