using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerSample : MonoBehaviour
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
        GameManager.InstanciateEnemy(new Vector2(8, 4), 1);
        GameManager.InstanciateEnemy(new Vector2(8, 0), 1);
        GameManager.InstanciateEnemy(new Vector2(3, 3), 1);
        GameManager.InstanciateEnemy(new Vector2(6, 1), 1);
        GameManager.InstanciateEnemy(new Vector2(7, 3), 1);
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
}
