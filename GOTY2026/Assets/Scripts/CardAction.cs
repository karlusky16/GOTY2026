using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardAction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image borde;
    Vector3 posicion;
    Vector3 scale;
    public GameObject centro;
    GameObject copia;
    public GameObject deck;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        centro = GameObject.Find("Visual Centrado");
        deck = GameObject.Find("InterfazJugador/CardPanel");
        scale = gameObject.GetComponent<RectTransform>().localScale;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(copia);
        if (eventData.button == PointerEventData.InputButton.Left && !Player.cartaSeleccionada)
        {
            Seleccionar();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DeSeleccionar();
        }
    }
    void Seleccionar()
    {
        Player.cartaSeleccionada = true;
        borde.color = Color.red;
        Player.carta = gameObject;
    }
    void DeSeleccionar()
    {
        Player.cartaSeleccionada = false;
        borde.color = Color.blue;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Player.cartaSeleccionada)
        {
            Destacar();
        }
    }

    void Destacar()
    {
        copia = Instantiate(gameObject);
        copia.transform.localScale = new Vector3(2f, 2f, 2f);
        copia.transform.SetParent(centro.transform, worldPositionStays: false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(copia);
    }


    internal void Efecto(Vector2[] tiles)
    {
        foreach (var dir in tiles)
        {
            if (GridManager._tiles.TryGetValue(dir, out Tile tile) && tile.ocupado && tile.ocupadoObj.CompareTag("Enemy"))
            {
                tile.ocupadoObj.GetComponent<EnemyController>().ReducirVida(5);
            }
        }
        switch (Player.carta.GetComponent<DisplayCard>().tipo)
        {
            case 0:
                player.GetComponent<PlayerController>().ReducirMana(Player.carta.GetComponent<DisplayCard>().coste);
                break;
            case 1:
                player.GetComponent<PlayerController>().ReducirEnergia(Player.carta.GetComponent<DisplayCard>().coste);
                break;
            case 2:
                player.GetComponent<PlayerController>().ReducirEnergia(Player.carta.GetComponent<DisplayCard>().coste);
                player.GetComponent<PlayerController>().ReducirMana(Player.carta.GetComponent<DisplayCard>().coste);
                break;
            default:
                break;
        }
        Player.carta = null;
        Player.cartaSeleccionada = false;
        Destroy(gameObject);
    /*else
    {
        Debug.Log("noMas");
        TurnManager.noMas.gameObject.SetActive(true);
        Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
    }*/
        
    }

    
    void OcultarMensaje()
    {
        TurnManager.noMas.gameObject.SetActive(false);
    }

    
    /*bool SePuede() {
        GameObject player = Player.carta.GetComponent<CardAction>().player;
        if ((Player.carta.GetComponent<DisplayCard>().tipo == 0 && player.GetComponent<PlayerController>().manaPlayer > 0)
            || (Player.carta.GetComponent<DisplayCard>().tipo == 1 && player.GetComponent<PlayerController>().energiaPlayer > 0)
            || (Player.carta.GetComponent<DisplayCard>().tipo == 2 && player.GetComponent<PlayerController>().energiaPlayer
            > 0 && player.GetComponent<PlayerController>().manaPlayer > 0))
        {
            return true;
        }
        else
            return false;
    }
    */
}

