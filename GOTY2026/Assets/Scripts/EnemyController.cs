using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

       

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player == null) {
            Debug.Log("Player no encontrado por EnemyController");
        }
    }
    public void Vida(int vida)
    {
        vidaMaximaEnemy = vida;
        vidaActualEnemy = vida;
        BarraVidaEnemy barra = GetComponentInChildren<BarraVidaEnemy>();
        barra.ConectarEnemy(this);
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
            if (GameManager.enemigos.Count == 0 && SceneManager.GetActiveScene().name=="CombateBoss")
            {
                GameObject.Find("SceneManager").SendMessage("LoadVictory");
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
            case "Caballero":
                if (TurnManager.numTurno == 0)
                {

                    GridManager._tiles.TryGetValue(new(0, 2), out Tile tile2);
                    int playerx =  GameManager.player.GetComponent<PlayerController>().GetPos().x;
                    if (!tile2.ocupado && playerx != 0)
                    {
                        Mover(new(0, 2));
                    }
                    else
                    {
                        Mover(new(1, 2));;
                    }

                }
                else
                {
                    Mover(CalcularRutaMasCorta(enemy, movimientos));
                }
                break;
            default:
                if(movimientos > 0) Mover(CalcularRutaMasCorta(enemy, movimientos));
                break;
        }
        }

    public void Mover(UnityEngine.Vector2 pos)
    {
        if (posicion != null){
            posicion.ocupado = false;
            posicion.ocupadoObj = null; 
        }
        posicion = GridManager._tiles[pos];
        posicion.ocupado = true;
        posicion.ocupadoObj = this.gameObject;
        gameObject.transform.position = new(posicion.transform.position.x,posicion.transform.position.y,-0.01f);
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
            //GameObject.FindGameObjectWithTag("Background").SendMessage("Desaparecer");
            //GameObject.FindGameObjectWithTag("Background").SendMessage("Aparecer");
        }
    }
/*Se mueve a la fila del player*/
    Vector2 MoverAFila(GameObject enemy)
    {
        int playery =  GameManager.player.GetComponent<PlayerController>().GetPos().y;
        int playerx =  GameManager.player.GetComponent<PlayerController>().GetPos().x;
        int enemyx = enemy.GetComponent<EnemyController>().GetPos().x;
        if(playery == enemy.GetComponent<EnemyController>().GetPos().y) return new Vector2(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y); // si ya está en la fila del player se queda donde estaba
        Vector2 nuevaPos = new Vector2(enemyx, playery);
        Tile tile;
        GridManager._tiles.TryGetValue(nuevaPos, out tile);
        if(tile.ocupado == false && enemyx != playerx) return nuevaPos; // moverlo desde el método Movimiento
        else
        {
            int ancho ;
            if(GameObject.Find("GridManager") == null)
            {
                FondoManager fondo = GameObject.Find("Fondo").GetComponent<FondoManager>();
                fondo.Aparecer();
                ancho = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
            }
            else
            {
                ancho = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
            }
            
            for(int i = 0; i < ancho - 1; i++)
            {
                if(enemyx == 0) enemyx = ancho;
                enemyx--;
                nuevaPos = new Vector2(enemyx, playery);
                GridManager._tiles.TryGetValue(nuevaPos, out tile);
                if(tile.ocupado == false && enemyx != playerx) return nuevaPos;
            }
            return new Vector2(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y) ; // aqui solo llega si ha acabado el for, por tanto i = ancho y como no se ha podido mover se queda en su posición original
            
        }

    }

    /*Calcula la ruta más corta y devulve la casilla a la que se mueve con sus x movimientos*/
    Vector2 CalcularRutaMasCorta(GameObject enemy, int movimientos)
    {
        int enemyx =enemy.GetComponent<EnemyController>().GetPos().x;
        int enemyy = enemy.GetComponent<EnemyController>().GetPos().y;
        int playerx = GameManager.player.GetComponent<PlayerController>().GetPos().x;
        int playery = GameManager.player.GetComponent<PlayerController>().GetPos().y;

        Vector2 start = new Vector2(enemyx, enemyy);
        Vector2 fin = new Vector2(playerx, playery);
        if (start == fin) return start;
        if (enemyx == playerx && ((playery == enemyy - 1) || (playery == enemyy + 1))) return start;
        if (enemyy == playery && ((playerx == enemyx - 1) || (playerx == enemyx + 1))) return start;
        Dictionary<Vector2, int> dist = new Dictionary<Vector2, int>();
        Dictionary<Vector2, Vector2> parent = new Dictionary<Vector2, Vector2>();
        List<Vector2> open = new List<Vector2>();
        dist[start] = 0;
        open.Add(start);

        Vector2[] dirs = new Vector2[]
        {
            new Vector2(1,0),
            new Vector2(-1,0),
            new Vector2(0,1),
            new Vector2(0,-1)
        };
        /*Dijkstra*/
        while (open.Count > 0)
        {
            Vector2 current = open[0];
            int bestDist = dist[current];
            for (int i = 1; i < open.Count; i++)
            {
                if (dist[open[i]] < bestDist)
                {
                    current = open[i];
                    bestDist = dist[current];
                }
            }
            open.Remove(current);
            if (current == fin) break;
            foreach (Vector2 dir in dirs)
            {
                Vector2 neighbor = current + dir;
                if (!GridManager._tiles.ContainsKey(neighbor)) continue;
                Tile tile = GridManager._tiles[neighbor];
                if (tile.ocupado && neighbor != fin) continue;
                int newDist = dist[current] + 1;
                if (!dist.ContainsKey(neighbor) || newDist < dist[neighbor])
                {
                    dist[neighbor] = newDist;
                    parent[neighbor] = current;
                    if (!open.Contains(neighbor))
                        open.Add(neighbor);
                }
            }
        }
        if (!parent.ContainsKey(fin))
        {
            return start;
        }
        List<Vector2> path = new List<Vector2>();
        Vector2 aux = fin;
        path.Add(aux);
        while (aux != start)
        {
            aux = parent[aux];
            path.Add(aux);
        }
        path.Reverse(); 
        if (movimientos <= 0)
            return start;
        if (movimientos >= path.Count)
            return path[path.Count - 2];
        return path[movimientos];   
        }
}

