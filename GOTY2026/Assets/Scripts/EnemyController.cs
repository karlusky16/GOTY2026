using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Action<int> EnemyReduceVida;
    [SerializeField] private int vidaMaximaEnemy;
    [SerializeField] private int vidaActualEnemy;
    public int dañoEnemy = 2;


    private PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        vidaActualEnemy = vidaMaximaEnemy;
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
    public void Ataque()
    {
        player.ReducirVida(dañoEnemy); 
        
    }
    public void Movimiento()
    {

    }
    
    void mostrarHighlight()
    {
        if (GameManager.cartaSeleccionada) {
            if(GameManager.enemy.GetComponent<DisplayEnemy>().patronAtaque == EnemyPattern.Cruz)
            {
                
            }
            
            }
        }
    }

