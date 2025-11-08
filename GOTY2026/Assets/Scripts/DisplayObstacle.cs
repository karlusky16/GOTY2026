using System;
using UnityEngine;

public class DisplayObstacle : MonoBehaviour
{
    public Obstacle obstacle;
    public int displayID;
    public int id;
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        obstacle = GameManager.obstacleList.Find(e => e.id == displayID);
        if (obstacle == null)
        {
            Debug.LogError($" Obstacle ID {displayID} no encontrado en ObstacleDatabase");
            return;
        }
    }


    void Update()
    {

    }
    public Obstacle GetObstacle() => obstacle;
    public int GetId() => obstacle.id;
    public String GetName() => obstacle.name;
}
