using UnityEngine;

public class SceneManager2 : MonoBehaviour
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
        GameManager.InstanciateEnemy(new Vector2(10, 4), 4);
        GameManager.InstanciateEnemy(new Vector2(9, 0), 5);
        GameManager.InstanciateEnemy(new Vector2(6, 1), 7);
        GameManager.InstanciateEnemy(new Vector2(6, 3), 7);
    }

    public void GenerarObstaculos()
    {
        GameManager.InstanciateObstacle(new Vector2(9, 4), 1);
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
    }
}
