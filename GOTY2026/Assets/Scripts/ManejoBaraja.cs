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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Inicializar()
    {
        //Buscamos el atributo player controller
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _image = GameObject.Find("InterfazUsuario/CardPanel");
        prefabCarta = GameObject.Find("GameManager").GetComponent<GameManager>().GetPrefabCarta();
                //copiamos la longitud de la dataBase
        int cartas = CardDataBase.cardList.Count;
        System.Random rand = new();
        if (mazoInicializado) return;
        mazoInicializado = true;
        //Para meter cartas aleatorias en la baraja del jugador
        for (int i = 0; i < 10; i++)
        {
            int carta = rand.Next(cartas);
            Debug.Log(carta);
            player.AddCarta(carta + 1);
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
            Debug.Log(player.GetCartas()[indiceAleatorio]);
            dc.ActualizarID(player.GetCartas()[indiceAleatorio]);
            player.GetCartas().RemoveAt(indiceAleatorio);
        }

    }
    //Para devolver las cartas no usadas al final del turno
    public static void DevolverMano()
    {   
        while (mano.Count > 0)
        {
            var carta = mano[0];
            DevolverCarta(carta);
            GameObject.Destroy(carta);
        }
    }
    //Para devolver la carta al usarse
    public static void DevolverCarta(GameObject carta)
    {
        mano.Remove(carta);
        player.AddCartaDescartes(carta.GetComponent<DisplayCard>().id);
    }
    // Update is called once per frame
    public static void Update()
    {
        
    }
}
