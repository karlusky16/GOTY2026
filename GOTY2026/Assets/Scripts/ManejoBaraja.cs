using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManejoBaraja : MonoBehaviour
{
    public GameObject prefabCarta;   // tu prefab de carta
    public static GameObject _image; //referencia al CardPanel
    public static PlayerController player;
    public static List<GameObject> mano = new();
    public GameObject descartesPadre, roboPadre;

    public static int[] mazoDefault = {7,8,8,1,7,3,3,26,27}; //1 fireballs,dos saltos, 1 espadazo y dos disparos
    public List<GameObject> GetMano() => mano;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Inicializar()
    {
        mano = new();
        //Buscamos el atributo player controller
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _image = GameObject.Find("InterfazUsuario/CardPanel");
        //copiamos la longitud de la dataBase
        int cartas = GameManager.cardList.Count;
        System.Random rand = new();
        if (player.stats.inicializado && player.GetCartasLength() > 0)
            return; // Ya hay cartas, no inicializamos
        player.stats.inicializado = true;
        //Para meter cartas aleatorias en la baraja del jugador
        foreach (var id in mazoDefault)
        {
            player.AddCarta(id);
        }
    }
    //Creación de la mano para cada turno
    public void ManoTurno()
    {
        System.Random rand = new();
        int manoC;
        //Generamos la mano aleatoriamente desde la lista de cartas
        if (player.GetLongMano() < player.GetCartasLength())
        {
            manoC = player.GetLongMano();
        }
        else
        {
            manoC = player.GetCartasLength();
        }
        for (int i = 0; i < manoC ; i++)
        {
            int cartas = TurnManager.robo.Count;
            if (cartas == 0)
            {
                DescartesABaraja();
                cartas = TurnManager.robo.Count;
            }
            int indiceAleatorio = rand.Next(cartas);
            mano.Add(TurnManager.robo[indiceAleatorio]);
            mano[i].transform.SetParent(_image.transform);
            TurnManager.robo.RemoveAt(indiceAleatorio);
        }
        TurnManager.interfaz.SetActive(true);
    }
    //Para devolver las cartas no usadas al final del turno
    public void DevolverMano()
    {
        while (mano.Count > 0)
        {
            var carta = mano[0];
            int idCarta = carta.GetComponent<DisplayCard>().GetCard().id;
            DevolverCarta(carta, idCarta,true);
        }
    }
    //Para devolver la carta al usarse
    public void DevolverCarta(GameObject carta, int id, bool devolver)
    {
        mano.Remove(carta);
        if (devolver)
        {
            AddCartaDescartes(carta);
        }
        else
        {
            Destroy(carta);
        }
    }
    // Update is called once per frame

    public void AddCartaDescartes(GameObject carta)
    {
        carta.transform.SetParent(descartesPadre.transform, false);
        TurnManager.descartes.Add(carta);
    }
    public void AddCartaRobo(int i)
    {
        for (int j = 0; j < i; j++)
        {
            RobarCarta();
        }
    }
    public void RobarCarta()
    {
        System.Random rand = new();
        int cartas = TurnManager.robo.Count;
        if (cartas == 0)
        {
            DescartesABaraja();
            cartas = TurnManager.robo.Count;
        }
        int indiceAleatorio = rand.Next(cartas);
        mano.Add(TurnManager.robo[indiceAleatorio]);
        mano[^1].transform.SetParent(_image.transform);
        TurnManager.robo.RemoveAt(indiceAleatorio);
    }
    public void DescartesABaraja()
    {
        Debug.Log("DescartesABaraja");
        TurnManager.robo = new List<GameObject>(TurnManager.descartes);
        for (int i = 0; i < TurnManager.robo.Count; i++)
        {
            TurnManager.robo[i].transform.SetParent(roboPadre.transform);
        }
        Debug.Log("Cartas en baraja: " + TurnManager.robo.Count);
        TurnManager.descartes.Clear();
    }

}
