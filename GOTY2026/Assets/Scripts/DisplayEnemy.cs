using System;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class DisplayEnemy : MonoBehaviour
{
    public Enemy enemy;
    public int displayID;
    public int id;
    public TextMeshProUGUI enemyDamageText;
    private Animator animator;
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
        gameObject.GetComponent<EnemyController>().Vida(enemy.vida);
        if(enemy.animator != null){
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = enemy.animator;
        }
    }
    private void Awake()
    {

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