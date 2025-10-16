using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int vidaEnemy = 10;
    public int dañoEnemy = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReducirVida(int vida)
    {
        if ((vidaEnemy -= vida) < 0) vidaEnemy = 0;
    }
    public void Ataque()
    {
        
    }
     public void Movimiento()
    {
        
    }
}
