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
        GameManager.InstanciateEnemy(new Vector2(7, 2), 6);
        GameManager.InstanciateEnemy(new Vector2(8, 2), 8);
        GameManager.InstanciateEnemy(new Vector2(8, 4), 5);
        GameManager.InstanciateEnemy(new Vector2(8, 0), 5);
    }

    public void GenerarObstaculos()
    {
        GameManager.InstanciateObstacle(new Vector2(7, 0), 1);
        GameManager.InstanciateObstacle(new Vector2(7, 4), 1);

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
        SceneManager.LoadScene("SelectCharacter");
    }
    public void LoadVictory()
    {
        victoryScreen.SetActive(true);
        
    }
    public void Salir()
    {
        Application.Quit();
    }
}
