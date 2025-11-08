using System;
using TMPro;
using UnityEngine;

public class DisplayEnemy : MonoBehaviour
{
    public Enemy enemy;
    public int displayID;
    public int id;
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        enemy = GameManager.enemyList.Find(e => e.id == displayID);
        if (enemy == null)
        {
            Debug.LogError($" Enemy ID {displayID} no encontrado en EnemyDataBase");
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = enemy.sprite;
    }


    void Update()
    {

    }
    public Enemy GetEnemy() => enemy;
    public int GetDaño() => enemy.daño;
    public String GetPatron() => enemy.patronAtaque;
    public int GetRango() => enemy.rango;
    public int GetArea() => enemy.area;

}