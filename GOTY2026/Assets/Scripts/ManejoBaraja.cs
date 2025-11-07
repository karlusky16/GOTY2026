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
    public static int[] mazoDefault = {7,7,8,8,9,9,10,1,2,3,3,4,5,6};
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
    public static void ManoTurno()
    {
        System.Random rand = new();
        //Generamos la mano aleatoriamente desde la lista de cartas
        for (int i = 0; i < player.GetLongMano(); i++)
        {
            int cartas = player.GetCartasLength();
            if (cartas == 0)
            {
                player.DescartesABaraja();
                cartas = player.GetCartasLength();
            }
            int indiceAleatorio = rand.Next(cartas);
            mano.Add(GameObject.Instantiate(prefabCarta, _image.transform));
            DisplayCard dc = mano[i].GetComponent<DisplayCard>();
            dc.ActualizarID(player.GetCartas()[indiceAleatorio]);
            player.GetCartas().RemoveAt(indiceAleatorio);
        }
        TurnManager.interfaz.SetActive(true);

    }
    //Para devolver las cartas no usadas al final del turno
    public static void DevolverMano()
    {   
        while (mano.Count > 0)
        {
            var carta = mano[0];
            int idCarta = carta.GetComponent<DisplayCard>().GetCard().id;
            DevolverCarta(carta,idCarta);
            GameObject.Destroy(carta);
        }
    }
    //Para devolver la carta al usarse
    public static void DevolverCarta(GameObject carta, int id)
    {
        player.AddCartaDescartes(id);
        mano.Remove(carta);
    }
    // Update is called once per frame
    public static void ResetBaraja()
    {
        foreach (var carta in mano)
        {
            if (carta != null)
            {
                Destroy(carta);
            }
        }
        mano.Clear();

        if (player != null)
        {
            player.ResetBaraja();
        }
        mazoInicializado = false;
        Inicializar();
        ManoTurno();
    }
}
