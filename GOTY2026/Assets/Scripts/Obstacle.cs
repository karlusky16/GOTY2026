using System;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Obstacle", menuName = "ScriptableObjects/Obstacle")]
public class Obstacle : ScriptableObject
{

    public int id;
    public String _name;
    public Sprite sprite;
    public int daño;
    public bool atravesable;
    public int turnosRestantes;
    public AnimatorController animator;
    
    //de momento estos dos como idea, no los voy a usar en esta iteración
    //public bool destruible;
    //public int vida;

    public Obstacle()
    {

    }
    public Obstacle(int id, String _name)
    {
        this.id = id;
        this._name = _name;  
    }
    
}
