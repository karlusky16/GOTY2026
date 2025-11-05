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
            GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
            GameManager.cartaSeleccionada = true;
            borde.color = Color.red;
            GameManager.carta = gameObject;
            GameObject.Find("GameManager").gameObject.SendMessage("MarcarRango",player.GetComponent<PlayerController>().GetPos());
            GameManager.CambiarLayerEnemy("Ignore Raycast");
        }

        if (eventData.button == PointerEventData.InputButton.Right && GameManager.cartaSeleccionada)
        {
            GameObject.Find("GameManager").gameObject.SendMessage("DesmarcarRango",player.GetComponent<PlayerController>().GetPos());
            GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
            GameManager.cartaSeleccionada = false;
            borde.color = gameObject.GetComponent<DisplayCard>().GetColor();
            GameManager.CambiarLayerEnemy("Default");
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
        switch (gameObject.GetComponent<DisplayCard>().GetTipo())
        {
            case 0: //ataque
                EfectoAtaque(tiles);
                break;
            case 1: //movimiento
                EfectoMovimiento(GameObject.Find("GameManager").GetComponent<TileManager>().GetTileMov()); //A medio hacer
                break;
            case 2: //ataque y movimiento
                EfectoMovimiento(tiles, GameObject.Find("GameManager").GetComponent<TileManager>().GetTileMov()); //A medio hacer
                break;
            case 3: //boosteo
                //EfectoBoosteo(tiles); //A medio hacer
                break;
            default:
                break;
        }
    }
    //Metodo para efecto de cartas que impliquen movimiento y ataque
    //Se debe pasar como parametro el tile al que se quiere mover el jugador y un array con las posiciones de ataque
    internal void EfectoMovimiento(Vector2[] tiles, Tile tile)
    {
        if (SePuede())
        {
            foreach (var dir in tiles)
            {

                if (GridManager._tiles.TryGetValue(dir, out Tile tile2) && tile2.ocupado && tile2.ocupadoObj.CompareTag("Enemy"))
                {
                    tile2.ocupadoObj.GetComponent<EnemyController>().ReducirVida(gameObject.GetComponent<DisplayCard>().GetDaño());
                }
            }
            player.GetComponent<PlayerController>().Mover(new(tile.x, tile.y));
            switch (GameManager.carta.GetComponent<DisplayCard>().GetTipoCoste())
            {
                case 0:
                    player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                case 1:
                    player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                case 2:
                    player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                default:
                    break;
            }
            ManejoBaraja.DevolverCarta(gameObject, gameObject.GetComponent<DisplayCard>().GetCard().id);
            GameManager.carta = null;
            GameManager.cartaSeleccionada = false;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("noMas");
            TurnManager.noMas.gameObject.SetActive(true);
            Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
        }
        GameManager.CambiarLayerEnemy("Default");
    }
    //Metodo para efecto de cartas que impliquen solo movimiento
    //Se debe pasar como parametro el tile al que se quiere mover el jugador
    internal void EfectoMovimiento(Tile tile)
    {
        if (SePuede())
        {
            player.GetComponent<PlayerController>().Mover(new(tile.x, tile.y));
            switch (GameManager.carta.GetComponent<DisplayCard>().GetTipoCoste())
            {
                case 0:
                    player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                case 1:
                    player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                case 2:
                    player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                default:
                    break;
            }
            ManejoBaraja.DevolverCarta(gameObject, gameObject.GetComponent<DisplayCard>().GetCard().id);
            GameManager.carta = null;
            GameManager.cartaSeleccionada = false;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("noMas");
            TurnManager.noMas.gameObject.SetActive(true);
            Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
        }
        GameManager.CambiarLayerEnemy("Default");
    }
    //Metodo para efecto de cartas que impliquen solo ataque
    //Se debe pasar como parametro un array con las posiciones de ataque
    internal void EfectoAtaque(Vector2[] tiles)
    {
        if (SePuede())
        {
            foreach (var dir in tiles)
            {

                if (GridManager._tiles.TryGetValue(dir, out Tile tile) && tile.ocupado && tile.ocupadoObj.CompareTag("Enemy"))
                {
                    tile.ocupadoObj.GetComponent<EnemyController>().ReducirVida(gameObject.GetComponent<DisplayCard>().GetDaño());
                }
            }
            switch (GameManager.carta.GetComponent<DisplayCard>().GetTipoCoste())
            {
                case 0:
                    player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                case 1:
                    player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                case 2:
                    player.GetComponent<PlayerController>().ReducirEnergia(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    player.GetComponent<PlayerController>().ReducirMana(GameManager.carta.GetComponent<DisplayCard>().GetCoste());
                    break;
                default:
                    break;
            }
            ManejoBaraja.DevolverCarta(gameObject, gameObject.GetComponent<DisplayCard>().GetCard().id);
            GameManager.carta = null;
            GameManager.cartaSeleccionada = false;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("noMas");
            TurnManager.noMas.gameObject.SetActive(true);
            Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
        }
        GameManager.CambiarLayerEnemy("Default");
    }

    
    void OcultarMensaje()
    {
        TurnManager.noMas.gameObject.SetActive(false);
    }

    
    bool SePuede() {
        GameObject player = GameManager.carta.GetComponent<CardAction>().player;
        int tipo = GameManager.carta.GetComponent<DisplayCard>().GetTipoCoste();
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

