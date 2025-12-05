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
    //Metodo para gestionar los clicks en la carta
    public void OnPointerClick(PointerEventData eventData)
    {
        //Si es click izquierdo y no hay ninguna carta seleccionada se selecciona la carta
        if ((eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right) && !GameManager.cartaSeleccionada)
        {
            //GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");//Se muestra el fondo
            GameManager.cartaSeleccionada = true;//Se marca la carta como seleccionada
            borde.color = Color.red;//Se cambia el color del borde para indicar que está seleccionada
            GameManager.carta = gameObject;//Se guarda la carta seleccionada en el GameManager
            GameObject.Find("GameManager").gameObject.SendMessage("MarcarRango", player.GetComponent<PlayerController>().GetPos());//Se marcan las tiles en rango
            GameManager.CambiarLayerEnemy("Ignore Raycast");//Se cambia la layer de los enemigos para que no interfieran con el click en las tiles
        }
        //Si es click derecho y hay una carta seleccionada se deselecciona la carta
        else if ((eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right) && GameManager.cartaSeleccionada && GameManager.carta == gameObject)
        {
            GameObject.Find("GameManager").gameObject.SendMessage("DesmarcarRango", player.GetComponent<PlayerController>().GetPos());//Se desmarcan las tiles en rango
            //GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");//Se oculta el fondo
            GameManager.cartaSeleccionada = false;//Se marca la carta como no seleccionada
            borde.color = gameObject.GetComponent<DisplayCard>().GetColor();//Se vuelve a poner el color original del borde
            GameManager.CambiarLayerEnemy("Default");//Se vuelve a poner la layer original de los enemigos
        }
        else if ((eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right) && GameManager.cartaSeleccionada && GameManager.carta != gameObject)
        {
            GameObject.Find("GameManager").gameObject.SendMessage("DesmarcarRango", player.GetComponent<PlayerController>().GetPos());//Se desmarcan las tiles en rango
            GameManager.cartaSeleccionada = false;//Se marca la carta como no seleccionada
            GameManager.carta.GetComponent<CardAction>().borde.color = GameManager.carta.GetComponent<DisplayCard>().GetColor();//Se vuelve a poner el color original del borde
            GameManager.CambiarLayerEnemy("Default");//Se vuelve a poner la layer original de los enemigos
            GameManager.cartaSeleccionada = true;//Se marca la carta como seleccionada
            borde.color = Color.red;//Se cambia el color del borde para indicar que está seleccionada
            GameManager.carta = gameObject;//Se guarda la carta seleccionada en el GameManager
            GameObject.Find("GameManager").gameObject.SendMessage("MarcarRango", player.GetComponent<PlayerController>().GetPos());//Se marcan las tiles en rango
            GameManager.CambiarLayerEnemy("Ignore Raycast");//Se cambia la layer de los enemigos para que no interfieran con el click en las tiles
        }
    }
    //Al poner el puntero encima de la carta se crea una copia para destacarla
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameManager.cartaSeleccionada)
        {
            Destacar();
        }
    }
    //Al salir del puntero se destruye la copia de la carta destacada
    public void OnPointerExit(PointerEventData eventData)
    {

        NoDestacar();
    }
    //Se crea una copia de la carta para destacarla
    void Destacar()
    {
        carta = Instantiate(gameObject);
        carta.transform.localScale = new Vector3(2f, 2f, 2f);
        carta.transform.SetParent(centro.transform, false);
    }
    //Se destruye la copia de la carta destacada
    void NoDestacar()
    {
        Destroy(carta);
    }
    //Metodo para aplicar el efecto de la carta dpendiendo de su tipo de efecto
    internal void Efecto(Vector2[] tiles)
    {
        borde.color = gameObject.GetComponent<DisplayCard>().GetColor();//Se vuelve a poner el color original del borde
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
        //Si se tiene suficiente recurso y la tile no está ocupada se realiza el efecto
        if (SePuede() && !tile.ocupado)
        {
            //Comprobación de enemigos en las tiles afectadas y reducción de vida
            foreach (var dir in tiles)
            {

                if (GridManager._tiles.TryGetValue(dir, out Tile tile2) && tile2.ocupado && tile2.ocupadoObj.CompareTag("Enemy"))
                {
                    tile2.ocupadoObj.GetComponent<EnemyController>().ReducirVida(gameObject.GetComponent<DisplayCard>().GetDaño());
                }
            }
            //Movimiento del jugador a la tile seleccionada
            player.GetComponent<PlayerController>().Mover(new(tile.x, tile.y));
            //Comprobación de tipo de coste y reducción del recurso correspondiente
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
            GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().DevolverCarta(gameObject, gameObject.GetComponent<DisplayCard>().GetCard().id);
            GameManager.carta = null;
            GameManager.cartaSeleccionada = false;
        }
        //Sino se muestra el mensaje correspondiente
        else
        {
            if (tile.ocupado)
            {
                Debug.Log("Tile ocupado");
                TurnManager.tileOcupada.gameObject.SetActive(true);
                Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
            }
            else
            {
                Debug.Log("noMas");
                TurnManager.noMas.gameObject.SetActive(true);
                Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
               
            }
            GameObject.Find("GameManager").gameObject.SendMessage("DesmarcarRango", player.GetComponent<PlayerController>().GetPos());//Se desmarcan las tiles en rango
            //GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");//Se oculta el fondo
            GameManager.cartaSeleccionada = false;//Se marca la carta como no seleccionada
            borde.color = gameObject.GetComponent<DisplayCard>().GetColor();//Se vuelve a poner el color original del borde
            GameManager.CambiarLayerEnemy("Default");//Se vuelve a poner la layer original de los enemigos
        }
        GameManager.CambiarLayerEnemy("Default");
    }
    //Metodo para efecto de cartas que impliquen solo movimiento
    //Se debe pasar como parametro el tile al que se quiere mover el jugador
    internal void EfectoMovimiento(Tile tile)
    {
        //Si se tiene suficiente recurso y la tile no está ocupada se realiza el efecto
        if (SePuede() && !tile.ocupado)
        {
            //Movimiento del jugador a la tile seleccionada
            player.GetComponent<PlayerController>().Mover(new(tile.x, tile.y));
            //Comprobación de tipo de coste y reducción del recurso correspondiente
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
            GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().DevolverCarta(gameObject, gameObject.GetComponent<DisplayCard>().GetCard().id);
            GameManager.carta = null;
            GameManager.cartaSeleccionada = false;
        }
        //Sino se muestra el mensaje correspondiente
        else
        {
            if (tile.ocupado)
            {
                TurnManager.tileOcupada.gameObject.SetActive(true);
                Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
                Debug.Log("Tile ocupado");

            }
            else
            {
                Debug.Log("noMas");
                TurnManager.noMas.gameObject.SetActive(true);
                Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
            }
            GameObject.Find("GameManager").gameObject.SendMessage("DesmarcarRango", player.GetComponent<PlayerController>().GetPos());//Se desmarcan las tiles en rango
            //GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");//Se oculta el fondo
            GameManager.cartaSeleccionada = false;//Se marca la carta como no seleccionada
            borde.color = gameObject.GetComponent<DisplayCard>().GetColor();//Se vuelve a poner el color original del borde
            GameManager.CambiarLayerEnemy("Default");//Se vuelve a poner la layer original de los enemigos
        }
        GameManager.CambiarLayerEnemy("Default");
    }
    //Metodo para efecto de cartas que impliquen solo ataque
    //Se debe pasar como parametro un array con las posiciones de ataque
    internal void EfectoAtaque(Vector2[] tiles)
    {
        //Si se tiene suficiente recurso se realiza el efecto
        if (SePuede())
        {
            //Comprobación de enemigos en las tiles afectadas y reducción de vida
            foreach (var dir in tiles)
            {

                if (GridManager._tiles.TryGetValue(dir, out Tile tile) && tile.ocupado && tile.ocupadoObj.CompareTag("Enemy"))
                {
                    tile.ocupadoObj.GetComponent<EnemyController>().ReducirVida(gameObject.GetComponent<DisplayCard>().GetDaño());
                    tile.ocupadoObj.GetComponent<EnemyController>().AddFuego(gameObject.GetComponent<DisplayCard>().GetDaño());
                    tile.ocupadoObj.GetComponent<EnemyController>().AddShock(gameObject.GetComponent<DisplayCard>().GetDaño());
                }
            }
            //Comprobación de tipo de coste y reducción del recurso correspondiente
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
            GameObject.Find("TurnManager").GetComponent<ManejoBaraja>().DevolverCarta(gameObject, gameObject.GetComponent<DisplayCard>().GetCard().id);
            GameManager.carta = null;
            GameManager.cartaSeleccionada = false;
        }
        //Sino se muestra el mensaje correspondiente
        else
        {
            Debug.Log("noMas");
            TurnManager.noMas.gameObject.SetActive(true);
            GameObject.Find("GameManager").gameObject.SendMessage("DesmarcarRango", player.GetComponent<PlayerController>().GetPos());//Se desmarcan las tiles en rango
            //GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");//Se oculta el fondo
            GameManager.cartaSeleccionada = false;//Se marca la carta como no seleccionada
            borde.color = gameObject.GetComponent<DisplayCard>().GetColor();//Se vuelve a poner el color original del borde
            GameManager.CambiarLayerEnemy("Default");//Se vuelve a poner la layer original de los enemigos
            Invoke(nameof(OcultarMensaje), 1f); // Llama a OcultarMensaje después de 1 segundo
        }
        GameManager.CambiarLayerEnemy("Default");
    }


    void OcultarMensaje()
    {
        TurnManager.noMas.gameObject.SetActive(false);
        TurnManager.tileOcupada.gameObject.SetActive(false);
    }


    bool SePuede()
    {
        GameObject player = GameManager.player;
        int coste = GameManager.carta.GetComponent<DisplayCard>().GetCoste();
        int tipo = GameManager.carta.GetComponent<DisplayCard>().GetTipoCoste();
        if ((tipo == 0 && player.GetComponent<PlayerController>().GetManaActual() - coste >= 0)
            || (tipo == 1 && player.GetComponent<PlayerController>().GetEnergiaActual() - coste >= 0)
            || (tipo == 2 && player.GetComponent<PlayerController>().GetEnergiaActual() - coste
            >= 0 && player.GetComponent<PlayerController>().GetManaActual() - coste >= 0))
        {
            return true;
        }
        else
            return false;
    }

}

