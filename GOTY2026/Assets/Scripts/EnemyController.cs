using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    public Action<int> EnemyReduceVida;
    [SerializeField] private int vidaMaximaEnemy;
    [SerializeField] private int vidaActualEnemy;
    private PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        
        vidaActualEnemy = vidaMaximaEnemy;
        BarraVidaEnemy barra = GetComponentInChildren<BarraVidaEnemy>();
        barra.ConectarEnemy(this);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vidaActualEnemy == 0)
        {
            Destroy(gameObject);
            GridManager._tiles[GameManager.enemigos[gameObject]].ocupadoObj = null;
            GridManager._tiles[GameManager.enemigos[gameObject]].ocupado = false;
            GameManager.enemigos.Remove(gameObject);
            GameManager.enemigosLis.Remove(gameObject);
            if (GameManager.enemigos.Count == 0)
            {
                GameManager.instance.victoryScreen.SetActive(true);
                Time.timeScale = 0f;
            }
            GridManager._tiles[new Vector2(1, 1)].ocupadoObj = null;
            GridManager._tiles[new Vector2(1, 1)].ocupado = false;
        }
    }
    public int GetVidaMaxima() => vidaMaximaEnemy;
    public int GetVidaActual() => vidaActualEnemy;
    public void ReducirVida(int vida)
    {
        if ((vidaActualEnemy -= vida) < 0) vidaActualEnemy = 0;
        EnemyReduceVida?.Invoke(vidaActualEnemy);
        Debug.Log("Reduce vida enemy");
        Debug.Log(vidaActualEnemy);
    }
    public void Ataque(Vector2[] posicionesAtaque, int dañoEnemy)
    {
        List<Vector2> posicionesAtaqueList = new List<Vector2>(posicionesAtaque);
        if (posicionesAtaqueList.Contains(new Vector2(player.GetPos().x, player.GetPos().y)))
        {
            player.ReducirVida(dañoEnemy);
            Debug.Log("Player atacado por enemy");
        }

    }
    public void Movimiento()
    {

    }

    void OnMouseEnter()
    {
        if (GameManager.cartaSeleccionada == false)
        {
            Debug.Log("Mouse encima enemy");
            GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
            gameObject.SendMessage("HighlightEnemyTiles", gameObject);
        }
    }
    
    void OnMouseExit()
    {
        if (GameManager.cartaSeleccionada == false )
        {
            Debug.Log("Mouse Sale enemy");
            gameObject.SendMessage("UnHighlightEnemyTiles");
            GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
        }
    }
}

