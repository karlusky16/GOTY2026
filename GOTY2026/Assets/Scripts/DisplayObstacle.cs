using System;
using UnityEngine;

public class DisplayObstacle : MonoBehaviour
{
    public Obstacle obstacle;
    public int displayID;
    public int id;
    public int turnosRestantes;
    public SpriteRenderer spriteRenderer;
    private Animator animator;
    public void ActualizarID(int nuevoDisplayID)
    {
        displayID = nuevoDisplayID;
        obstacle = GameManager.obstacleList.Find(e => e.id == displayID);
        if (obstacle == null)
        {
            Debug.LogError($" Obstacle ID {displayID} no encontrado en ObstacleDatabase");

            return;
        }
        spriteRenderer.sprite = obstacle.sprite;
        turnosRestantes = obstacle.turnosRestantes;
        if(obstacle.animator != null){
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = obstacle.animator;
        }
    }
    private void Awake()
    {
    }
    void Update()
    {

    }
    public Obstacle GetObstacle() => obstacle;
    public int GetId() => obstacle.id;
    public String GetName() => obstacle.name;
}
