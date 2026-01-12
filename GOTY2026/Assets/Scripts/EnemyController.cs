using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public Action<int> EnemyReduceVida;
    [SerializeField] private int vidaMaximaEnemy;
    [SerializeField] private int vidaActualEnemy;
    private PlayerController player;
    public Tile posicion;
    public int danoFuego;
    public Boolean shock;
    public Image fuego;
    public Image aturdido;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player == null)
        {
            Debug.Log("Player no encontrado por EnemyController");
        }
        shock = false;
        fuego.gameObject.SetActive(false);
        aturdido.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
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
                if (SceneManager.GetActiveScene().name == "CombateBoss")
                {
                    GameManager.player.transform.position = new Vector2(-1000, 0);
                    GameObject.Find("SceneManager").SendMessage("LoadVictory");
                }
                else
                {
                    if (SceneManager.GetActiveScene().name != "CombateE2" && SceneManager.GetActiveScene().name != "CombateE1")
                    {
                        GameManager.player.transform.position = new Vector2(-1000, 0);
                        SceneManager.LoadScene("Recompensas");
                    }
                    else
                    {
                        GameManager.player.transform.position = new Vector2(-1000, 0);
                        SceneManager.LoadScene("RecompensasElite");
                    }
                }
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
    public void AddFuego(int dano)
    {
        if (dano <= 0) return;
        danoFuego += dano;
        if (!fuego.gameObject.activeSelf) fuego.gameObject.SetActive(true);
        fuego.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = danoFuego.ToString();
        Debug.Log("Enemy recibe fuego");
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
    public void ResetShock()
    {
        shock = false;
        aturdido.gameObject.SetActive(false);
    }
    public void Fuego()
    {
        if (danoFuego > 0)
        {
            if (gameObject.GetComponent<DisplayEnemy>().GetName() == "Dragon")
            {
                ReducirVida(danoFuego-- / 2);
            }
            else
            {
                ReducirVida(danoFuego--);
            }
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
    public void RedFuego()
    {
        danoFuego = 0;
        fuego.gameObject.SetActive(false);
    }
    public void Ataque(Vector2[] posicionesAtaque, int dañoEnemy, int danoFE, int sVE)
    {
        if (shock)
        {
            Debug.Log("Enemy aturdido, no puede atacar");
            shock = false;
        }
        else
        {
            List<Vector2> posicionesAtaqueList = new(posicionesAtaque);
            if (posicionesAtaqueList.Contains(new Vector2(player.GetPos().x, player.GetPos().y)))
            {
                player.ReducirVida(dañoEnemy);
                player.AddFuego(danoFE);
                player.AddShock(sVE);
                Debug.Log("Player atacado por enemy");
            }
            if (gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 1 || gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 6 || gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 9)
            {
                for (int i = 0; i < posicionesAtaqueList.Count; i++)
                {
                    var ataque = Instantiate(gameObject.GetComponent<DisplayEnemy>().GetEnemy().prefabAtaque, new(posicionesAtaqueList[i].x, posicionesAtaqueList[i].y,(float) -1.0), Quaternion.identity);
                    Destroy(ataque, 1f);
                }
            }
        }
    }
    public void Movimiento(GameObject enemy)
    {
        int movimientos = enemy.GetComponent<DisplayEnemy>().GetMovimiento();
        String name = enemy.GetComponent<DisplayEnemy>().GetName();

        switch (name)
        {
            case "Robot" or "Dragon":
                Mover(MoverAFila(enemy));
                break;
            case "Caballero":
                if (TurnManager.numTurno == 0)
                {

                    GridManager._tiles.TryGetValue(new(0, 2), out Tile tile2);
                    int playerx = GameManager.player.GetComponent<PlayerController>().GetPos().x;
                    if (!tile2.ocupado && playerx != 0)
                    {
                        Mover(new(0, 2));
                    }
                    else
                    {
                        Mover(new(1, 2)); ;
                    }

                }
                else
                {
                    Mover(CalcularRutaMasCorta(enemy, movimientos));
                }
                break;
            default:
                if (movimientos > 0) Mover(CalcularRutaMasCorta(enemy, movimientos));
                break;
        }
    }

    public void Mover(UnityEngine.Vector2 pos)
    {
        if (posicion != null)
        {
            posicion.ocupado = false;
            posicion.ocupadoObj = null;
        }
        if (gameObject.GetComponent<DisplayEnemy>().GetName() == "Dragon")
        {
            Tile posAnt = posicion;
            posicion = GridManager._tiles[pos];
            if (posicion.ocupado)
            {
                GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                GameManager.obstacles.Remove(posicion.ocupadoObj);
                GameManager.obstaclesLis.Remove(posicion.ocupadoObj);
                Destroy(posicion.ocupadoObj);
                posicion = GridManager._tiles[pos];
                posicion.ocupado = true;
                posicion.ocupadoObj = this.gameObject;
                gameObject.transform.position = new(posicion.transform.position.x, posicion.transform.position.y, -0.1f);
                GameManager.enemigos[gameObject] = pos;
                gm.InstanciateObstacle(new Vector2(posAnt.x, posAnt.y), 0);
            }
            else
            {
                posicion = GridManager._tiles[pos];
                posicion.ocupado = true;
                posicion.ocupadoObj = this.gameObject;
                gameObject.transform.position = new(posicion.transform.position.x, posicion.transform.position.y, -0.1f);
                GameManager.enemigos[gameObject] = pos;
            }
        }
        else
        {
            posicion = GridManager._tiles[pos];
            posicion.ocupado = true;
            posicion.ocupadoObj = this.gameObject;
            gameObject.transform.position = new(posicion.transform.position.x, posicion.transform.position.y, -0.1f);
            GameManager.enemigos[gameObject] = pos;
        }

    }

    void OnMouseEnter()
    {
        if (GameManager.cartaSeleccionada == false && !shock && !GameObject.Find("TurnManager").GetComponent<TurnManager>().viendoTE)
        {
            gameObject.SendMessage("HighlightEnemyTiles", gameObject);
        }
    }

    void OnMouseExit()
    {
        if (GameManager.cartaSeleccionada == false && !shock && !GameObject.Find("TurnManager").GetComponent<TurnManager>().viendoTE)
        {
            gameObject.SendMessage("UnHighlightEnemyTiles");
        }
    }
    public void OnMouseDown()
    {
        Debug.Log("Mouse click enemy");
        BestiarioManager bm = GameObject.Find("PanelInfo").GetComponent<BestiarioManager>();
        bm.CambiarEstado();
        if (bm.RetEnP())
        {
            bm.DisplayDatos(gameObject);
        }
    }
    /*Se mueve a la fila del player*/
    Vector2 MoverAFila(GameObject enemy)
    {
        int playery = GameManager.player.GetComponent<PlayerController>().GetPos().y;
        int playerx = GameManager.player.GetComponent<PlayerController>().GetPos().x;
        int enemyx = enemy.GetComponent<EnemyController>().GetPos().x;
        if (playery == enemy.GetComponent<EnemyController>().GetPos().y) return new Vector2(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y); // si ya está en la fila del player se queda donde estaba
        Vector2 nuevaPos = new(enemyx, playery);
        Tile tile;
        GridManager._tiles.TryGetValue(nuevaPos, out tile);
        if (tile.ocupado == false && enemyx != playerx) return nuevaPos; // moverlo desde el método Movimiento
        if (tile.ocupado && tile.ocupadoObj.CompareTag("Obstacle") && tile.ocupadoObj.GetComponent<DisplayObstacle>().GetId() == 0)
        {
            Debug.Log("Estoy Aqui");
            return nuevaPos;
        }
        else
        {
            int ancho;
            if (GameObject.Find("GridManager") == null)
            {
                FondoManager fondo = GameObject.Find("Fondo").GetComponent<FondoManager>();
                fondo.Aparecer();
                ancho = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
            }
            else
            {
                ancho = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
            }

            for (int i = 0; i < ancho - 1; i++)
            {
                if (enemyx == 0) enemyx = ancho;
                enemyx--;
                nuevaPos = new Vector2(enemyx, playery);
                GridManager._tiles.TryGetValue(nuevaPos, out tile);
                if (tile.ocupado == false && enemyx != playerx) return nuevaPos;
            }
            return new Vector2(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y); // aqui solo llega si ha acabado el for, por tanto i = ancho y como no se ha podido mover se queda en su posición original

        }

    }

    /*Calcula la ruta más corta y devulve la casilla a la que se mueve con sus x movimientos*/
    Vector2 CalcularRutaMasCorta(GameObject enemy, int movimientos)
    {
        int enemyx = enemy.GetComponent<EnemyController>().GetPos().x;
        int enemyy = enemy.GetComponent<EnemyController>().GetPos().y;
        int playerx = GameManager.player.GetComponent<PlayerController>().GetPos().x;
        int playery = GameManager.player.GetComponent<PlayerController>().GetPos().y;

        Vector2 start = new(enemyx, enemyy);
        Vector2 fin = new(playerx, playery);
        if (start == fin) return start;
        if (enemyx == playerx && ((playery == enemyy - 1) || (playery == enemyy + 1))) return start;
        if (enemyy == playery && ((playerx == enemyx - 1) || (playerx == enemyx + 1))) return start;
        Dictionary<Vector2, int> dist = new Dictionary<Vector2, int>();
        Dictionary<Vector2, Vector2> parent = new Dictionary<Vector2, Vector2>();
        List<Vector2> open = new();
        dist[start] = 0;
        open.Add(start);

        Vector2[] dirs = new Vector2[]
        {
            new(1,0),
            new(-1,0),
            new(0,1),
            new(0,-1)
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
        List<Vector2> path = new();
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
        if (movimientos >= path.Count - 1)
            return path[path.Count - 2];
        return path[movimientos];
    }
    public void HacerObstaculos(Vector2[] posicionesAtaque)
    {
        List<Vector2> posicionesAtaqueList = new(posicionesAtaque);
        if (gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 5 || gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 8 || (gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 10 && gameObject.GetComponent<TileManagerEnemigo>().patronDragon == 1))
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            for (int i = 0; i < posicionesAtaqueList.Count; i++)
            {
                if (GridManager._tiles.TryGetValue(posicionesAtaqueList[i], out Tile t))
                {
                    if (!t.ocupadoAt)
                    {
                        if (t.ocupado && t.ocupadoObj.CompareTag("Player"))
                        {
                            GameManager.player.GetComponent<PlayerController>().Mover(new(t.x, t.y));
                        }
                        gm.InstanciateObstacle(posicionesAtaqueList[i], 2);
                    }
                }
            }
        }
        else if (gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 6 || (gameObject.GetComponent<DisplayEnemy>().GetEnemy().id == 10 && gameObject.GetComponent<TileManagerEnemigo>().patronDragon == 6))
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            for (int i = 0; i < posicionesAtaqueList.Count; i++)
            {
                if (GridManager._tiles.TryGetValue(posicionesAtaqueList[i], out Tile t))
                {
                    if (!t.ocupadoAt)
                    {
                        if (t.ocupado && t.ocupadoObj.CompareTag("Player"))
                        {
                            GameManager.player.GetComponent<PlayerController>().Mover(new(t.x, t.y));
                        }
                        gm.InstanciateObstacle(posicionesAtaqueList[i], 3);
                    }
                }
            }
        }
    }
}

