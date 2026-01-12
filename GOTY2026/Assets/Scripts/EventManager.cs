using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static List<Events> eventList;
    public Events eventoActual;
    public EventUI eventUI;
    public DisplayCard cartaMendigo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        eventList = new List<Events>(Resources.LoadAll<Events>("Events"));
        if (eventList.Count == 0)
        {
            Debug.LogError("No hay eventos en Assets/Resources/Events");
        }
    }
    void Start()
    {
        if (eventList == null || eventList.Count == 0) return;
        eventoActual = GetRandomEvent();
        eventUI.Show(eventoActual);
    }
    Events GetRandomEvent()
    {
        int i = UnityEngine.Random.Range(0, eventList.Count);
        return eventList[i];
    }
    public void boton1()
    {
        String name = eventUI.getNombre();
        switch (name)
        {
            case "Curandero":
                Curandero1();
                break;
            case "Duende":
                Duende1();
                break;
            case "Mago":
                Mago1();
                break;
            case "Mendigo":
                Mendigo1();
                break;
            default:
                break;

        }
    }
    public void boton2()
    {
        String name = eventUI.getNombre();
        switch (name)
        {
            case "Curandero":
                Curandero2();
                break;
             case "Duende":
                Duende2();
                break;
            case "Mago":
                Mago2();
                break;
            case "Mendigo":
                Mendigo2();
                break;
            default:
                break;

        }
    }
    public void salir(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    public void BotonVolver()
    {
        eventUI.mensajePoción.SetActive(false);
        eventUI.mensajeCarta.SetActive(false);
    }

    private void Curandero1()
    {
        GameManager.player.GetComponent<PlayerController>().ReducirMonedas(10);
        eventUI.ActualizarTextoMonedas();
        GameManager.player.GetComponent<PlayerController>().AumentarVida(2);
        eventUI.boton1.interactable = false;
        eventUI.ComprobarCosteBotones();
    }
    private void Curandero2()
    {
        GameManager.player.GetComponent<PlayerController>().ReducirMonedas(50);
        eventUI.ActualizarTextoMonedas();
        GameManager.player.GetComponent<PlayerController>().AumentarVidaMaxima(3);
        eventUI.boton2.interactable = false;
        eventUI.ComprobarCosteBotones();
    }
    private void Duende1()
    {
        GameManager.player.GetComponent<PlayerController>().ReducirMonedas(40);
        eventUI.ActualizarTextoMonedas();
        GameManager.player.GetComponent<PlayerController>().AumentarEnergiaMaxima(1);
        eventUI.boton1.interactable = false;
        eventUI.boton2.interactable = false;
    }
    private void Duende2()
    {
        GameManager.player.GetComponent<PlayerController>().ReducirManaMaxima(1);
        GameManager.player.GetComponent<PlayerController>().AumentarEnergiaMaxima(1);
        eventUI.boton2.interactable = false;
        eventUI.ComprobarCosteBotones();
    }
    private void Mago1()
    {
        GameManager.player.GetComponent<PlayerController>().ReducirMonedas(40);
        eventUI.ActualizarTextoMonedas();
        GameManager.player.GetComponent<PlayerController>().AumentarManaMaxima(1);
        eventUI.boton1.interactable = false;
        eventUI.boton1.interactable = false;
    }
    private void Mago2()
    {
        int aleatorio = UnityEngine.Random.Range(1,3);
        if(aleatorio == 1)
        {
            GameManager.player.GetComponent<PlayerController>().ReducirVida(2);
            GameManager.player.GetComponent<PlayerController>().ReducirVidaMaxima(2);
            eventUI.mensajePoción.SetActive(true);
            eventUI.mensajeResta.SetActive(true);
        }
        else
        {
            GameManager.player.GetComponent<PlayerController>().AumentarVidaMaxima(2);
            GameManager.player.GetComponent<PlayerController>().AumentarVida(2);
            eventUI.mensajePoción.SetActive(true);
            eventUI.mensajeSuma.SetActive(true);
        }
        eventUI.boton2.interactable = false;
        eventUI.ComprobarCosteBotones();    
    }
    private void Mendigo1()
    {
        int randomIndex = UnityEngine.Random.Range(0, GameManager.cardList.Count);
        int idCarta = GameManager.cardList[randomIndex].id;
        GameManager.player.GetComponent<PlayerController>().AddCarta(idCarta);
        eventUI.mensajeCarta.SetActive(true);
        cartaMendigo.ActualizarID(idCarta);
        eventUI.boton1.interactable = false;
        
    }
    private void Mendigo2()
    {
        GameManager.player.GetComponent<PlayerController>().ReducirMonedas(10);
        eventUI.ActualizarTextoMonedas();
        eventUI.ComprobarCosteBotones();
    }
}
