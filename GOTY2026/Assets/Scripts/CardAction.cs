using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardAction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image borde;
    public GameObject carta;
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
        if (eventData.button == PointerEventData.InputButton.Left && !Player.cartaSeleccionada)
        {
            Player.cartaSeleccionada = true;
            borde.color = Color.red;
            Player.carta = gameObject;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Player.cartaSeleccionada = false;
            borde.color = Color.blue;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Player.cartaSeleccionada) {
           carta.transform.localScale = new Vector3(1f, 1f, 1f);
          // carta.transform.position = new Vector3(posicion.x, posicion.y, posicion.z);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        carta.transform.localScale = scale;
        //carta.transform.position = posicion;
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

    
    /*void OcultarMensaje()
    {
        TurnManager.noMas.gameObject.SetActive(false);
    }*/

    
    /*bool SePuede() {
        GameObject player = Player.carta.GetComponent<CardAction>().player;
        if ((Player.carta.GetComponent<DisplayCard>().tipo == 0 && player.GetComponent<PlayerController>().manaPlayer > 0)
            || (Player.carta.GetComponent<DisplayCard>().tipo == 1 && player.GetComponent<PlayerController>().energiaPlayer > 0)
            || (Player.carta.GetComponent<DisplayCard>().tipo == 2 && player.GetComponent<PlayerController>().energiaPlayer
            > 0 && player.GetComponent<PlayerController>().manaPlayer > 0))
        {
            return true;
        }
        Player.carta = null;
        Player.cartaSeleccionada = false;
        Destroy(gameObject);
    }
    */
}

