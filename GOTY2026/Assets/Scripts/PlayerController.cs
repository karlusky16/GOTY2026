using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int vidaPlayer = 10;
    public int manaPlayer = 5;
    public int energiaPlayer = 5;
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
        if ((vidaPlayer -= vida) < 0) vidaPlayer = 0;
    }

    public void ReducirEnergia(int energia)
    {
        if ((energiaPlayer -= energia) < 0) energiaPlayer = 0;
    }
    
     public void ReducirMana(int mana)
    {
        if ((manaPlayer -= mana) < 0) manaPlayer = 0;
    }
}
