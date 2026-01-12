using System.Data.Common;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class EventUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text textoDescripcion;
    public TMP_Text textoBoton1;
    public TMP_Text textoBoton2;
    public TMP_Text textMonedas;
    public Button boton1;
    public Button boton2;
    private Events events;
    public GameObject mensajePoción;
    public GameObject mensajeSuma;
    public GameObject mensajeResta;
    public GameObject mensajeCarta;


    public void Show(Events evento)
    {
        nameText.text = evento.nombre;
        textoDescripcion.text = evento.texto;
        textoBoton1.text = evento.textBoton1;
        textoBoton2.text = evento.textBoton2;
        events = evento;
        ComprobarCosteBotones();
        ActualizarTextoMonedas();
    }
    
    public String getNombre() => events.nombre;
    public String getTexto() => events.texto;
    public String getTextoBoton1() => events.textBoton1;
    public String getTextoBoton2() => events.textBoton2;
    public int getCosteMonedas1() => events.costeMonedas1;
    public int getCosteMonedas2() => events.costeMonedas2;
    public int getCosteEnergia1() => events.costeEnergia1;
    public int getCosteEnergia2() => events.costeEnergia2;
    public int getCosteMana1() => events.costeMana1;
    public int getCosteMana2() => events.costeMana2;
    public int getCosteVida1() => events.costeVida1;
    public int getCosteVida2() => events.costeVida2;

    public void ActualizarTextoMonedas()
    {
        int monedasJugador = GameManager.player.GetComponent<PlayerController>().GetMonedas();
        textMonedas.text = "Monedas: " + monedasJugador.ToString();
    }
    public void ComprobarCosteBotones()
    {
        if(events.costeMonedas1 > GameManager.player.GetComponent<PlayerController>().GetMonedas()) boton1.interactable = false;
        if(events.costeMonedas2 > GameManager.player.GetComponent<PlayerController>().GetMonedas()) boton2.interactable = false;
        if(events.costeEnergia1 > GameManager.player.GetComponent<PlayerController>().GetEnergiaMaxima()) boton1.interactable = false;
        if(events.costeEnergia2 > GameManager.player.GetComponent<PlayerController>().GetEnergiaMaxima()) boton2.interactable = false;
        if(events.costeMana1 > GameManager.player.GetComponent<PlayerController>().GetManaMaxima()) boton1.interactable = false;
        if(events.costeMana2 > GameManager.player.GetComponent<PlayerController>().GetManaMaxima()) boton2.interactable = false;
        if(events.costeVida1> GameManager.player.GetComponent<PlayerController>().GetVidaMaxima()) boton1.interactable = false;
        if(events.costeVida2 > GameManager.player.GetComponent<PlayerController>().GetVidaMaxima()) boton2.interactable = false;
    }
}
