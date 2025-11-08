using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Obstacle", menuName = "ScriptableObjects/Obstacle")]
public class Obstacle : ScriptableObject
{

    public int id;
    public String _name;
    
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
