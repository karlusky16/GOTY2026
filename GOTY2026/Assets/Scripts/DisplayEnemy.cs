using System;
using TMPro;
using UnityEngine;

public class DisplayEnemy : MonoBehaviour
{
    public Enemy enemy;
    public int displayID;
    public int id;
    public String _name;
    public int vida;
    public int daño;
    public EnemyPattern patronAtaque;


    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        enemy = EnemyDataBase.enemyList.Find(e => e.id == displayID);
        if (enemy == null)
        {
            Debug.LogError($" Enemy ID {displayID} no encontrado en EnemyDataBase");
            return;
        }
    }


    void Update()
    {
        this.id = enemy.id;
        this._name = enemy._name;
        this.vida = enemy.vida;
        this.daño = enemy.daño;
        this.patronAtaque = enemy.patronAtaque;
    }

}