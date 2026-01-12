using System;
using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
public class Enemy : ScriptableObject
{
    public int id;
    public String _name;
    public int vida;
    public int daño;
    public int dañoFuego;
    public int shockValue;
    public int rango;
    public int area;
    public int movimiento;
    public String patronAtaque;
    public AnimatorController animator;
    public GameObject prefabAtaque;
    
}

/*
Aqui van los patrones de pueda tener cada enemigo
*/
