using System;
using TMPro;
using UnityEngine;

public class DisplayEnemy : MonoBehaviour
{
    public Enemy enemy;
    public int displayID;
    public int id;
    public TextMeshProUGUI enemyDamageText;
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        enemy = GameManager.enemyList.Find(e => e.id == displayID);
        if (enemy == null)
        {
            Debug.LogError($" Enemy ID {displayID} no encontrado en EnemyDataBase");
            return;
        }
        enemyDamageText.text = "Daño : " + enemy.daño;
        gameObject.GetComponent<SpriteRenderer>().sprite = enemy.sprite;
        gameObject.GetComponent<EnemyController>().Vida();
    }


    void Update()
    {

    }
    public Enemy GetEnemy() => enemy;
    public int GetDaño() => enemy.daño;
    public String GetPatron() => enemy.patronAtaque;
    public int GetRango() => enemy.rango;
    public int GetArea() => enemy.area;
    public int GetMovimiento() => enemy.movimiento;
    public String GetName() => enemy._name;

}