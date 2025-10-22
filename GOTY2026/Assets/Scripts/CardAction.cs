using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardAction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image borde;
    public static GameObject carta;
    Vector3 posicion;
    Vector3 scale;
    public GameObject centro;
    GameObject copia;
    public GameObject deck;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.player;
        centro = GameObject.Find("Visual Centrado");
        deck = GameObject.Find("InterfazJugador/CardPanel");
        scale = gameObject.GetComponent<RectTransform>().localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !GameManager.cartaSeleccionada)
        {
            GameManager.cartaSeleccionada = true;
            borde.color = Color.red;
            GameManager.carta = gameObject;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.cartaSeleccionada = false;
            borde.color = Color.blue;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameManager.cartaSeleccionada)
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
        carta.transform.localScale = new Vector3(2f,2f,2f);
        carta.transform.SetParent(centro.transform, false);
    }
     void NoDestacar()
    {
        Destroy(carta);
    }


    internal void Efecto(Vector2[] tiles)
    {
        if (SePuede())
        {
            foreach (var dir in tiles)
            {

                if (GridManager._tiles.TryGetValue(dir, out Tile tile) && tile.ocupado && tile.ocupadoObj.CompareTag("Enemy"))
                {
                    tile.ocupadoObj.GetComponent<EnemyController>().ReducirVida(5);
                }
                switch (GameManager.carta.GetComponent<DisplayCard>().tipo)
                {
                    case 0:
                        player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().coste);
                        break;
                    case 1:
                        player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().coste);
                        break;
                    case 2:
                        player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().coste);
                        player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().coste);
                        break;
                    default:
                        break;
                }
            }
            GameManager.carta = null;
            GameManager.cartaSeleccionada = false;
            ManejoBaraja.DevolverCarta(gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("noMas");
            TurnManager.noMas.gameObject.SetActive(true);
            Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
        }
    }

    
    void OcultarMensaje()
    {
        TurnManager.noMas.gameObject.SetActive(false);
    }

    
    bool SePuede() {
        GameObject player = GameManager.carta.GetComponent<CardAction>().player;
        int tipo = GameManager.carta.GetComponent<DisplayCard>().tipo;
        if (( tipo == 0 && player.GetComponent<PlayerController>().GetManaActual() > 0)
            || (tipo == 1 && player.GetComponent<PlayerController>().GetEnergiaActual() > 0)
            || (tipo == 2 && player.GetComponent<PlayerController>().GetEnergiaActual()
            > 0 && player.GetComponent<PlayerController>().GetManaActual() > 0))
        {
            return true;
        }
        else
            return false;
    }
    
}

