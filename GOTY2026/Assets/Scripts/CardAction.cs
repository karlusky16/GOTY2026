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
            TurnManager.cartaSeleccionada = true;
            borde.color = Color.red;
            TurnManager.carta = gameObject;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TurnManager.cartaSeleccionada = false;
            borde.color = Color.blue;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!TurnManager.cartaSeleccionada)
        {
            Destacar();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        NoDestacar();
    }

    void Destacar()
    {
        carta = Instantiate(gameObject);
        carta.transform.localScale = new Vector2(2f,2f);
        carta.transform.SetParent(centro.transform, false);
    }
     void NoDestacar()
    {
        Destroy(carta);
    }


    internal void Efecto(Vector2[] tiles)
    {
        /*if (SePuede())
        {*/
        Debug.Log($"--- DEBUG EFECTO ---");
        Debug.Log($"TurnManager.carta: {TurnManager.carta}");
        Debug.Log($"DisplayCard: {TurnManager.carta.GetComponent<DisplayCard>()}");
        Debug.Log($"Player: {player}");
        Debug.Log($"Tiene PlayerController: {player.GetComponent<PlayerController>()}");
        Debug.Log($"Tiles: {GridManager._tiles}");
        foreach (var dir in tiles)
        {
            if (GridManager._tiles.TryGetValue(dir, out Tile tile) && tile.ocupado && tile.ocupadoObj.CompareTag("Enemy"))
            {
                tile.ocupadoObj.GetComponent<EnemyController>().ReducirVida(5);
            }
            switch (TurnManager.carta.GetComponent<DisplayCard>().tipo)
            {
                case 0:
                    player.GetComponent<PlayerController>().ReducirMana(TurnManager.carta.GetComponent<DisplayCard>().coste);
                    break;
                case 1:
                    player.GetComponent<PlayerController>().ReducirEnergia(TurnManager.carta.GetComponent<DisplayCard>().coste);
                    break;
                case 2:
                    player.GetComponent<PlayerController>().ReducirEnergia(TurnManager.carta.GetComponent<DisplayCard>().coste);
                    player.GetComponent<PlayerController>().ReducirMana(TurnManager.carta.GetComponent<DisplayCard>().coste);
                    break;
                default:
                    break;
            }
            //}
            /*else
            {
                Debug.Log("noMas");
                TurnManager.noMas.gameObject.SetActive(true);
                Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
            }*/
        }
        TurnManager.carta = null;
        TurnManager.cartaSeleccionada = false;
        Destroy(gameObject);
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
        else
            return false;
    }*/
    //}
    
}

