using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Player : MonoBehaviour
{
    public static List<int> cartas = new();
    public static int longMano = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AddCarta(int id)
    {
        cartas.Add(id);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
