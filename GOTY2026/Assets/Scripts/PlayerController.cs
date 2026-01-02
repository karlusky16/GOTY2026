using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static List<int> cartas = new();
    public static List<int> pasivos = new();
    public PlayerStats stats;
    public Tile posicion;
    public GameObject mirilla;
    public static int longMano;
    public Action<int> JugadorReduceVida;
    public Action<int> JugadorAumentaVida;

    public Action<int> JugadorReduceEnergia;
    public Action<int> JugadorAumentaEnergia;

    public Action<int> JugadorReduceMana;
    public Action<int> JugadorAumentaMana;

    [SerializeField] private int vidaMaxima;
    [SerializeField] private int vidaActual;

    [SerializeField] private int energiaMaxima;
    [SerializeField] private int energiaActual;

    [SerializeField] private int manaMaxima;
    [SerializeField] private int manaActual;
    [SerializeField] private int monedas;
    public Image fuego, aturdido, escudoS;
    public int danoFuego;
    public int escudo;
    public bool shock = false;
    public bool apuntado = false;
    public Boolean inicializado;
    public int dañoTemp; //Daño adicional que se reinicia al final del turno
    public int dañoItems; //Daño adicional permanente por items
    public int shockTemp; //Shock adicional que se reinicia al final del turno
    public int valorAturdidoItems; //Valor de aturdido adicional permanente por items
    public int escudoItems; //Escudo adicional permanente por items
    public int dañoFuegoItems; //Daño de fuego adicional permanente por items
    private void Awake()
    {
        stats ??= new PlayerStats();
        inicializado = stats.inicializado;
        CargarStats(stats);
        danoFuego = 0;
        escudo = 0;
        dañoTemp = 0;
        dañoItems = 0;
    }
    public void CargarStats(PlayerStats stats2)
    {
        this.stats = stats2;
        inicializado = stats.inicializado;
        longMano = stats.longMano;
        vidaMaxima = stats.vidaMaxima;
        vidaActual = stats.vidaActual;
        energiaMaxima = stats.energiaMaxima;
        manaMaxima = stats.manaMaxima;
        monedas = stats.monedas;
        cartas = stats.cartas;
        energiaActual = energiaMaxima;
        manaActual = manaMaxima;
        pasivos = stats.pasivos;
        dañoItems = stats.dañoItems;
        escudoItems = stats.escudoItems;
        dañoFuegoItems = stats.danoFuegoItems;
        valorAturdidoItems = stats.valorAturdidoItems;
    }
    public void GuardarStats()
    {
        stats.inicializado = this.inicializado;
        stats.longMano = longMano;
        stats.vidaMaxima = vidaMaxima;
        stats.vidaActual = vidaActual;
        stats.energiaMaxima = energiaMaxima;
        stats.manaMaxima = manaMaxima;
        stats.monedas = monedas;
        stats.cartas = cartas;
        stats.pasivos = pasivos;
        stats.dañoItems = dañoItems;
        stats.escudoItems = escudoItems;
        stats.danoFuegoItems = dañoFuegoItems;
        stats.valorAturdidoItems = valorAturdidoItems;
    }
    public void Mover(UnityEngine.Vector2 pos)
    {
        if (posicion != null)
        {
            posicion.ocupado = false;
            posicion.ocupadoObj = null;
        }
        posicion = GridManager._tiles[pos];
        posicion.ocupado = true;
        posicion.ocupadoObj = this.gameObject;
        gameObject.transform.position = new(posicion.transform.position.x, posicion.transform.position.y, 0);
    }
    //Getters
    public Tile GetPos() => posicion;
    public int GetCartasLength() => cartas.Count;
    public int GetLongMano() => longMano;
    public List<int> GetCartas() => cartas;
    public int GetVidaMaxima() => vidaMaxima;
    public int GetVidaActual() => vidaActual;
    public int GetEnergiaMaxima() => energiaMaxima;
    public int GetEnergiaActual() => energiaActual;
    public int GetManaMaxima() => manaMaxima;
    public int GetManaActual() => manaActual;
    public int GetMonedas() => monedas;

    //Modificar vida del jugador
    public void ReducirVida(int vida)
    {
        int vidDE = 0;
        if ((vidDE = RedEscudo(vida)) < 0)
        {
            vida = -vidDE;
        }
        else
        {
            vida = 0;
        }
        if ((vidaActual -= vida) <= 0)
        {
            vidaActual = 0;
            GameObject.Find("SceneManager").SendMessage("LoadGameOver");
            Debug.Log("Jugador muerto");

        }
        JugadorReduceVida?.Invoke(vidaActual);
        Debug.Log("Reduce vida jugador");

    }
    public void AumentarVida(int vida)
    {
        if ((vidaActual += vida) > vidaMaxima) vidaActual = vidaMaxima;
        JugadorAumentaVida?.Invoke(vidaActual);
        Debug.Log("Aumenta vida jugador");
    }

    public void AddEscudo(int escudoAdd)
    {
        if(escudoAdd == 0)  return;
        escudo += escudoAdd;
        if (!escudoS.gameObject.activeSelf)
            escudoS.gameObject.SetActive(true);
        escudoS.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = escudo.ToString();
        Debug.Log("Player recibe fuego");
    }
    public int RedEscudo(int daño)
    {
        int res = escudo - daño;
        if ((escudo -= daño) <= 0)
        {
            escudo = 0;
            escudoS.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
            escudoS.gameObject.SetActive(false);
        }
        return res;
    }
    //Modificar energia del jugador
    public void ReducirEnergia(int energia)
    {
        if ((energiaActual -= energia) < 0) energiaActual = 0;
        JugadorReduceEnergia?.Invoke(energiaActual);
        Debug.Log("Reduce energia jugador");
    }
    public void AumentarEnergia(int energia)
    {
        if ((energiaActual += energia) > energiaMaxima) energiaActual = energiaMaxima;
        JugadorAumentaEnergia?.Invoke(energiaActual);
        Debug.Log("Aumenta energia jugador");
    }

    //Modificar mana del jugador
    public void ReducirMana(int mana)
    {
        if ((manaActual -= mana) < 0) manaActual = 0;
        JugadorReduceMana?.Invoke(manaActual);
        Debug.Log("Reduce mana jugador");
    }
    public void AumentarMana(int mana)
    {
        if ((manaActual += mana) > manaMaxima) manaActual = manaMaxima;
        JugadorAumentaMana?.Invoke(manaActual);
        Debug.Log("Aumenta mana jugador");
    }
    public void AumentarMonedas(int cantidad)
    {
        monedas += cantidad;
    }
    public void ReducirMonedas(int cantidad)
    {
        monedas -= cantidad;
        if (monedas < 0) monedas = 0;
    }
    public void AddCarta(int id)
    {
        Debug.Log("AddCarta " + id);
        cartas.Add(id);
    }

    public void RemoveCarta(int id)
    {
        Debug.Log("RemoveCarta " + id);
        cartas.Remove(id);
    }

    public void ResetPlayer()
    {
        energiaActual = energiaMaxima;
        JugadorAumentaEnergia?.Invoke(energiaActual);
        manaActual = manaMaxima;
        JugadorAumentaMana?.Invoke(manaActual);
        vidaActual = vidaMaxima;
        JugadorAumentaVida?.Invoke(vidaActual);
    }
    public void Mirilla()
    {
        Debug.Log("Activar mirilla");
        apuntado = true;
        mirilla.SetActive(true);
    }
    public void ResetMirilla()
    {
        Debug.Log("Desactivar mirilla");
        apuntado = false;
        mirilla.SetActive(false);
    }
    public void AddFuego(int dano)
    {
        if (dano == 0) return;
        danoFuego += dano;
        if (!fuego.gameObject.activeSelf)
            fuego.gameObject.SetActive(true);
        fuego.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = danoFuego.ToString();
        Debug.Log("Player recibe fuego");
    }
    public void RedFuego(int dano)
    {
        if ((danoFuego -= dano) < 0) danoFuego = 0;
        if (danoFuego == 0)
        {
            fuego.gameObject.SetActive(false);
        }
        else
        {
            fuego.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = danoFuego.ToString();
        }
    }
    public void AddShock(int valor)
    {
        System.Random rand = new();
        int probabilidad = rand.Next(0, 100);
        if (probabilidad < valor)
        {
            aturdido.gameObject.SetActive(true);
            shock = true;
            Debug.Log("Enemy aturdido");
        }
    }
    public void Fuego()
    {
        if (danoFuego > 0)
        {
            ReducirVida(danoFuego--);
            if (danoFuego == 0)
            {
                fuego.gameObject.SetActive(false);
            }
            else
            {
                fuego.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = danoFuego.ToString();
            }
            Debug.Log("Enemy recibe daño por fuego");
        }
        else
        {
            fuego.gameObject.SetActive(false);
        }
    }
    public void OnMouseDown()
    {
        Debug.Log("Mouse click Player");
        GameObject.Find("PanelInfo").SendMessage("CambiarEstado");
        if (GameObject.Find("PanelInfo").GetComponent<BestiarioManager>().RetEnP())
        {
            GameObject.Find("PanelInfo").SendMessage("DisplayDatos", this.gameObject);
        }
    }
    public void AñadirPasivo(int id)
    {
        pasivos.Add(id);
        ActivarPasivos(id);
    }
    public void ActivarPasivos(int id)
    {
        switch (id)
        {
            case 0:
                vidaMaxima += 5;
                vidaActual += 5;
                JugadorAumentaVida?.Invoke(vidaActual);
                break;
            case 1:
                energiaMaxima += 2;
                manaMaxima -= 2;
                break;
            case 2:
                manaMaxima += 2;
                manaActual -= 2;
                break;
            case 3:
                dañoItems += 1;
                break;
            case 4:
                escudoItems += 2;
                break;
            case 5:
                dañoFuegoItems += 1;
                break;
            case 6:
                valorAturdidoItems += 5;
                break;
            default:
                Debug.LogError("Pasivo no reconocido");
                break;
        }
    }
    public void ResetTemporales()
    {
        dañoTemp = 0;
        shockTemp = 0;
    }
}