using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManejoBaraja : MonoBehaviour
{
    public static GameObject prefabCarta;   // tu prefab de carta
    public static GameObject _image; //referencia al CardPanel
    public static PlayerController player;
    public static List<GameObject> mano = new();
    static Boolean mazoInicializado = false;
    public  GameObject descartesPadre,roboPadre;
    public static int[] mazoDefault = { 7, 7, 8, 8, 9, 9, 10, 1, 2, 3, 3, 4, 5, 6 };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Inicializar()
    {
        //Buscamos el atributo player controller
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _image = GameObject.Find("InterfazUsuario/CardPanel");
        prefabCarta = GameObject.Find("GameManager").GetComponent<GameManager>().GetPrefabCarta();
        //copiamos la longitud de la dataBase
        int cartas = GameManager.cardList.Count;
        System.Random rand = new();
        if (mazoInicializado) return;
        mazoInicializado = true;
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
        //Generamos la mano aleatoriamente desde la lista de cartas
        for (int i = 0; i < player.GetLongMano(); i++)
        {
            int cartas = TurnManager.robo.Count;
            if (cartas == 0)
            {
                DescartesABaraja();
                cartas = player.GetCartasLength();
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
            DevolverCarta(carta, idCarta);
        }
    }
    //Para devolver la carta al usarse
    public void DevolverCarta(GameObject carta, int id)
    {
        AddCartaDescartes(carta);
        mano.Remove(carta);
    }
    // Update is called once per frame

    public void AddCartaDescartes(GameObject carta)
    {
        carta.transform.SetParent(descartesPadre.transform,false);
        TurnManager.descartes.Add(carta);
    }
    public void DescartesABaraja()
    {
        Debug.Log("DescartesABaraja");
        TurnManager.robo = new List<GameObject>(TurnManager.descartes);
        for (int i = 0; i< TurnManager.robo.Count;i++) {
            TurnManager.robo[i].transform.SetParent(descartesPadre.transform,false);
        }
        Debug.Log("Cartas en baraja: " + TurnManager.robo.ToString());
        TurnManager.descartes.Clear();
    }
    
}
