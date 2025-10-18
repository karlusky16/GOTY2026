using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int vidaEnemy = 3;
    public int dañoEnemy = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        
    }
     public void Movimiento()
    {
        
    }
}
