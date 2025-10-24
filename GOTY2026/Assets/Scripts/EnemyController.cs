using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int vidaEnemy = 3;
    public int dañoEnemy = 2;


    private PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(vidaEnemy == 0)
        {
            Destroy(gameObject);
            GridManager._tiles[new Vector2(1, 1)].ocupadoObj = null;
            GridManager._tiles[new Vector2(1, 1)].ocupado = false;
        }
    }
    public void ReducirVida(int vida)
    {
        if ((vidaEnemy -= vida) < 0) vidaEnemy = 0;
        Debug.Log("reducido");
        Debug.Log(vidaEnemy);
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

