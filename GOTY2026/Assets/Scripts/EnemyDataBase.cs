using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDataBase : MonoBehaviour
{
    public static List<Enemy> enemyList = new();
    void Awake()
    {
        enemyList.Add(new Enemy(1, "normal", 5, 2, EnemyPattern.Cruz));
    }
    
}
