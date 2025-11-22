using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public Action<int> EnemyReduceVida;
    [SerializeField] private int vidaMaximaEnemy;
    [SerializeField] private int vidaActualEnemy;
    private PlayerController player;
    public Tile posicion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        
        vidaActualEnemy = vidaMaximaEnemy;
        BarraVidaEnemy barra = GetComponentInChildren<BarraVidaEnemy>();
        barra.ConectarEnemy(this);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vidaActualEnemy == 0)
        {
            Destroy(gameObject);
            GridManager._tiles[GameManager.enemigos[gameObject]].ocupadoObj = null;
            GridManager._tiles[GameManager.enemigos[gameObject]].ocupado = false;
            GameManager.enemigos.Remove(gameObject);
            GameManager.enemigosLis.Remove(gameObject);
            if (GameManager.enemigos.Count == 0)
            {
                SceneManager.LoadScene("Recompensas");
            }
        }
    }
    public int GetVidaMaxima() => vidaMaximaEnemy;
    public int GetVidaActual() => vidaActualEnemy;
    public Tile GetPos() => posicion;
    public void ReducirVida(int vida)
    {
        if ((vidaActualEnemy -= vida) < 0) vidaActualEnemy = 0;
        EnemyReduceVida?.Invoke(vidaActualEnemy);
        Debug.Log("Reduce vida enemy");
        Debug.Log(vidaActualEnemy);
    }
    public void Ataque(Vector2[] posicionesAtaque, int dañoEnemy)
    {
        List<Vector2> posicionesAtaqueList = new List<Vector2>(posicionesAtaque);
        if (posicionesAtaqueList.Contains(new Vector2(player.GetPos().x, player.GetPos().y)))
        {
            player.ReducirVida(dañoEnemy);
            Debug.Log("Player atacado por enemy");
        }

    }
    public void Movimiento(GameObject enemy)
    {
        int movimientos = enemy.GetComponent<DisplayEnemy>().GetMovimiento();
        String name = enemy.GetComponent<DisplayEnemy>().GetName();

        switch (name)
        {
            case "Robot":
                Mover(MoverAFila(enemy));
                break;
            default:
                break;
        }
        }

    public void Mover(UnityEngine.Vector2 pos)
    {
        posicion = GridManager._tiles[pos];
        gameObject.transform.position = posicion.transform.position;
        GameManager.enemigos[gameObject] = pos;
    }

    void OnMouseEnter()
    {
        if (GameManager.cartaSeleccionada == false)
        {
            Debug.Log("Mouse encima enemy");
            GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
            gameObject.SendMessage("HighlightEnemyTiles", gameObject);
        }
    }
    
    void OnMouseExit()
    {
        if (GameManager.cartaSeleccionada == false )
        {
            Debug.Log("Mouse Sale enemy");
            gameObject.SendMessage("UnHighlightEnemyTiles");
            GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
        }
    }
/*Se mueve a la fila del player*/
    Vector2 MoverAFila(GameObject enemy)
    {
        int playery =  GameManager.player.GetComponent<PlayerController>().GetPos().y;
        int enemyx = enemy.GetComponent<EnemyController>().GetPos().x;
        if(playery == enemy.GetComponent<EnemyController>().GetPos().y) return new Vector2(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y); // si ya está en la fila del player se queda donde estaba
        Vector2 nuevaPos = new Vector2(enemyx, playery);
        Tile tile;
        GridManager._tiles.TryGetValue(nuevaPos, out tile);
        if(tile.ocupado == false) return nuevaPos; // moverlo desde el método Movimiento
        else
        {
            int ancho = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
            for(int i = 0; i < ancho - 1; i++)
            {
                if(enemyx == 0) enemyx = ancho;
                enemyx--;
                nuevaPos = new Vector2(enemyx, playery);
                GridManager._tiles.TryGetValue(nuevaPos, out tile);
                if(tile.ocupado == false) return nuevaPos;
            }
            return new Vector2(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y) ; // aqui solo llega si ha acabado el for, por tanto i = ancho y como no se ha podido mover se queda en su posición original
            
        }

    }

    

    void CalcularRutaMasCorta()
    {
        
    }
}

