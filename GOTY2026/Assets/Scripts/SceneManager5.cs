using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager5 : MonoBehaviour
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
        GameManager.InstanciateEnemy(new Vector2(5, 0), 4);
        GameManager.InstanciateEnemy(new Vector2(6, 4), 5);
        GameManager.InstanciateEnemy(new Vector2(6, 2), 6);
        GameManager.InstanciateEnemy(new Vector2(7, 3), 8);
    }

    public void GenerarObstaculos()
    {
        GameManager.InstanciateObstacle(new Vector2(7, 0), 1);
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
        StartCoroutine(ResetCoroutine());
    }
    private IEnumerator ResetCoroutine()
    {
        GameManager.reset = true;

        if (GameManager.player != null)
            Destroy(GameManager.player);

        if (GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);

        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            File.Delete(Application.persistentDataPath + "/save.json");
            Debug.Log("Archivo de guardado eliminado");
        }

        // Espera un frame para que Destroy se ejecute
        yield return null;

        SceneManager.LoadScene("MenuPrincipal");
    }
    public void Salir()
    {
#if UNITY_EDITOR
        // Si est�s en el editor, detiene el Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si est�s en un build (EXE, Mac, etc.), cierra la aplicaci�n
        Application.Quit();
#endif
    }
}
