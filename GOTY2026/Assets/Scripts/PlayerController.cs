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
    public static List<int> descartes = new();
    public Tile posicion;
    public GameObject mirilla;
    public static int longMano = 6;
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
    public Image fuego;
    public Image aturdido;
    public int danoFuego = 0;
    public bool shock = false;
    public bool apuntado = false;

    private void Awake()
    {
        vidaActual = vidaMaxima;
        energiaActual = energiaMaxima;
        manaActual = manaMaxima;
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
    public void AddCartaDescartes(int id)
    {
        Debug.Log("AddCartaDescartes: " + id);
        descartes.Add(id);
    }
    public void DescartesABaraja()
    {
        Debug.Log("DescartesABaraja");
        cartas = new List<int>(descartes);
        Debug.Log("Cartas en baraja: " + cartas.ToString());
        descartes.Clear();
    }

    public void ResetBaraja()
    {
        if (cartas != null)
        {
            cartas.Clear();
        }
        if (descartes != null)
        {
            descartes.Clear();
        }
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
        danoFuego += dano;
        if (!fuego.gameObject.activeSelf)
            fuego.gameObject.SetActive(true);
        fuego.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = danoFuego.ToString();
        Debug.Log("Player recibe fuego");
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
        Debug.Log("Mouse click enemy");
        GameObject.Find("PanelInfo").SendMessage("CambiarEstado");
        GameObject.Find("PanelInfo").SendMessage("DisplayDatos",this.gameObject);
    }
}