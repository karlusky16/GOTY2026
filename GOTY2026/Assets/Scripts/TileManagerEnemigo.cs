using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileManagerEnemigo : MonoBehaviour
{
    public Vector2[] direccionesAnt;
    private String lastPatron;
    public int patronDragon = 0;
    public Vector2[] GetRango() => direccionesAnt;
    public Boolean visto = false;
    public GameObject lanzaPrefab;
    public Vector2[] circulo = { new(1, 0), new(-1, 0), new(1, -1), new(1, 1), new(-1, 1), new(-1, -1), new(0, 1), new(0, -1) };
    public void CalculoTiles(GameObject enemy)
    {
        lastPatron = enemy.GetComponent<DisplayEnemy>().GetPatron();
        switch (lastPatron)
        {
            case "Aleatorios":
                direccionesAnt = AleatoriosPatron(enemy.GetComponent<DisplayEnemy>().GetArea());
                break;
            case "RectaHorizontal":
                direccionesAnt = RectaHorizontal(enemy.GetComponent<DisplayEnemy>().GetArea(),
                new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y),
                new(GameObject.Find("Player").GetComponent<PlayerController>().GetPos().x, GameObject.Find("Player").GetComponent<PlayerController>().GetPos().y));
                break;
            case "Caballero":
                if (TurnManager.numTurno == 0)
                {
                    direccionesAnt = Caballero1(enemy.GetComponent<DisplayEnemy>().GetArea(),
                    new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                }
                else
                {
                    direccionesAnt = Circulo(enemy.GetComponent<DisplayEnemy>().GetArea(),
                    new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                }
                break;
            case "Sniper":
                break;
            case "Mortero":
                direccionesAnt = Mortero(new(GameObject.Find("Player").GetComponent<PlayerController>().GetPos().x, GameObject.Find("Player").GetComponent<PlayerController>().GetPos().y));
                break;
            case "Golem":
                direccionesAnt = Golem(new(GameObject.Find("Player").GetComponent<PlayerController>().GetPos().x, GameObject.Find("Player").GetComponent<PlayerController>().GetPos().y));
                break;
            case "Misilero":
                direccionesAnt = Misilero(enemy.GetComponent<DisplayEnemy>().GetArea(),
                new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                break;
            case "Cruz":
                direccionesAnt = Cruz(enemy.GetComponent<DisplayEnemy>().GetArea(), enemy);
                break;
            case "Circulo":
                direccionesAnt = Circulo(enemy.GetComponent<DisplayEnemy>().GetArea(), new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                break;
            case "Bomba":
                if(TurnManager.numTurno == 2) direccionesAnt = TodoElTablero();
                if(TurnManager.numTurno == 3) Destroy(enemy);
                break;
            case "Dragon":
                patronDragon = PatronDragon(enemy.GetComponent<EnemyController>().GetPos());
                switch (patronDragon)
                {
                    case 1:
                        ModificarDatosAtaque(2,2,0,enemy);
                        direccionesAnt = AlientoFuego(enemy.GetComponent<DisplayEnemy>().GetArea(),new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                        break;
                    case 2:
                        ModificarDatosAtaque(10,0,0,enemy);
                        direccionesAnt = Circulo(enemy.GetComponent<DisplayEnemy>().GetArea(), new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                        break;
                    case 3:
                        ModificarDatosAtaque(5,0,50,enemy);
                        direccionesAnt = RayosParalizantes(enemy.GetComponent<DisplayEnemy>().GetArea(),new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                        break;
                    case 4:
                        ModificarDatosAtaque(7,0,0,enemy);
                        direccionesAnt = LatigoCola(new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y));
                        break;
                    case 5:
                        ModificarDatosAtaque(2,0,50,enemy);
                        break;
                    case 6:
                        ModificarDatosAtaque(4,0,0,enemy);
                        direccionesAnt = MorteroD();
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
    public void HighlightEnemyTiles(GameObject enemy)
    {
        if (enemy.GetComponent<DisplayEnemy>().GetPatron() == "Sniper" || (enemy.GetComponent<DisplayEnemy>().GetPatron() == "Dragon" && patronDragon == 5))
        {
            GameObject.Find("Player").GetComponent<PlayerController>().Mirilla();
        }
        else
        {
            foreach (Vector2 dir in direccionesAnt)
            {
                if (GridManager._tiles.TryGetValue(dir, out Tile tile2))
                    tile2.gameObject.SendMessage("HighlightEnemy");
            }
        }
    }
    public void UnHighlightEnemyTiles()
    {
        if (direccionesAnt == null)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ResetMirilla();
        }
        else
        {
           foreach (var dir in direccionesAnt)
            {
                if (GridManager._tiles.TryGetValue(dir, out Tile tile2))
                {
                    tile2.gameObject.SendMessage("UnHighlightEnemy");
                }
            } 
        }
        
    }
    public int PatronDragon(Tile tile)
    {
        Vector2 posPlayer = new(GameManager.player.GetComponent<PlayerController>().posicion.x, GameManager.player.GetComponent<PlayerController>().posicion.y);
        Vector2 posEnemy = new(tile.x, tile.y);
        Vector2 distancia = posPlayer - posEnemy;
        bool mele = false;
        if (distancia.x < 2 && distancia.x > -2 && distancia.y < 2 && distancia.y > -2)
        {
            Debug.Log("Estoy a mele");
            mele = true;
        }
        System.Random rand = new();
        int num = rand.Next(1, 3);
        int num2 = rand.Next(1, 6);
        int[] numsM = { 2, 4, 5 };
        int[] numsD = { 1, 1, 3, 5, 6, 6 };
        if (mele)
        {
            num = numsM[num];
        }
        else
        {
            num = numsD[num2];
        }
        return num;
    }
    public void ModificarDatosAtaque(int daño, int dañoFuego, int shock, GameObject enemy)
    {
        enemy.GetComponent<DisplayEnemy>().enemy.daño = daño;
        enemy.GetComponent<DisplayEnemy>().enemy.dañoFuego = dañoFuego;
        enemy.GetComponent<DisplayEnemy>().enemy.shockValue = shock;
    }
    public Vector2[] AlientoFuego(int area,Vector2 posEnemigo)
    {
        List<Vector2> direcciones = new();
        for (int i = 1; i < area; i++)
        {
            if (i == 1)
            {
                direcciones.Add(posEnemigo + new Vector2(-i, 0));
                direcciones.Add(posEnemigo + new Vector2(-i, i));
                direcciones.Add(posEnemigo + new Vector2(-i, -i));
            }
            else
            {
                direcciones.Add(posEnemigo + new Vector2(-i, 0));
                direcciones.Add(posEnemigo + new Vector2(-i, 1));
                direcciones.Add(posEnemigo + new Vector2(-i, 2));
                direcciones.Add(posEnemigo + new Vector2(-i, -1));
                direcciones.Add(posEnemigo + new Vector2(-i, -2));
            }
        }
        return direcciones.ToArray(); 
    }
    public Vector2[] RayosParalizantes(int area,Vector2 posEnemigo)
    {
        List<Vector2> direcciones = new();
        for (int i = 1; i < area; i++)
        {
            if (i == 1)
            {
                direcciones.Add(posEnemigo + new Vector2(-i, 0));
                direcciones.Add(posEnemigo + new Vector2(-i, i));
                direcciones.Add(posEnemigo + new Vector2(-i, 2 * i));
                direcciones.Add(posEnemigo + new Vector2(-i, 3 * i));
                direcciones.Add(posEnemigo + new Vector2(-i, 4 * i));
                direcciones.Add(posEnemigo + new Vector2(-i, 5 * i));
                direcciones.Add(posEnemigo + new Vector2(-i, -i));
                direcciones.Add(posEnemigo + new Vector2(-i, 2 * -i));
                direcciones.Add(posEnemigo + new Vector2(-i, 3 * -i));
                direcciones.Add(posEnemigo + new Vector2(-i, 4 * -i));
                direcciones.Add(posEnemigo + new Vector2(-i, 5 * -i));
            }
            else
            {
                direcciones.Add(posEnemigo + new Vector2(-i, 0));
            }
        }
        return direcciones.ToArray(); 
    }
    public Vector2[] LatigoCola(Vector2 posEnemigo)
    {
        List<Vector2> direcciones = new();
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                direcciones.Add(posEnemigo + new Vector2(i, 1));
                direcciones.Add(posEnemigo + new Vector2(i, 2));
                direcciones.Add(posEnemigo + new Vector2(i, -1));
                direcciones.Add(posEnemigo + new Vector2(i, -2));
            }
            else
            {
                direcciones.Add(posEnemigo + new Vector2(-i, 0));
                direcciones.Add(posEnemigo + new Vector2(-i, 1));
                direcciones.Add(posEnemigo + new Vector2(-i, 2));
                direcciones.Add(posEnemigo + new Vector2(-i, -1));
                direcciones.Add(posEnemigo + new Vector2(-i, -2));
            }
        }
        return direcciones.ToArray();
    }
    public Vector2[] AleatoriosPatron(int area)
    {
        HashSet<Vector2> direcciones = new();
        System.Random rand = new();
        int x = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
        int y = GameObject.Find("GridManager").GetComponent<GridManager>().GetHeight();
        while (direcciones.Count < area)
        {
            int direccionX = rand.Next(0, x);
            int direccionY = rand.Next(0, y);
            direcciones.Add(new Vector2(direccionX, direccionY));
        }
        direccionesAnt = direcciones.ToArray();
        return direccionesAnt;
    }
    public Vector2[] MorteroD()
    {
        HashSet<Vector2> direcciones = new();
        System.Random rand = new();
        int x = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth()-1;
        int y = GameObject.Find("GridManager").GetComponent<GridManager>().GetHeight()-1;
        while (direcciones.Count < 6*4)
        {
            int direccionX = rand.Next(0, x);
            int direccionY = rand.Next(0, y);
            direcciones.Add(new Vector2(direccionX, direccionY));
            direcciones.Add(new(direccionX + 1, direccionY ));
            direcciones.Add(new(direccionX, direccionY + 1));
            direcciones.Add(new(direccionX + 1, direccionY + 1));
        }
        direccionesAnt = direcciones.ToArray();
        return direccionesAnt;
    }
    /*Para el robot, ya que ataca solo en su fila*/
    private static Vector2[] RectaHorizontal(int area, Vector2 enemy, Vector2 player) // tile la casilla seleccionada
    {

        List<Vector2> direcciones = new();
        int playerx = (int)player.x; // para atacar hacia el enemigo
        int enemyx = (int)enemy.x;
        bool plaDer = false;
        int x = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
        if (enemyx < 0 || enemyx >= x) { Debug.Log("Posición del enemy en RectaHorizontal en TileMEnemigo"); return null; }
        if (playerx < 0 || playerx >= x) { Debug.Log("Posición del player en RectaHorizontal en TileMEnemigo"); return null; }

        if (playerx - enemyx > 0) plaDer = true;
        int cont = 0;

        if (plaDer)
        {
            for (int i = enemyx + 1; i < x && i >= 0; i++)
            {
                direcciones.Add(new(i, enemy.y));
                cont++;
                if (cont == area) break;
            }
        }
        else
        {
            for (int i = enemyx - 1; i < x && i >= 0; i--)
            {
                direcciones.Add(new(i, enemy.y));
                cont++;
                if (cont == area) break;
            }
        }
        return direcciones.ToArray();
    }
    public static Vector2[] Caballero1(int area, Vector2 enemy)
    {
        // Removed GridManager lookup: not needed for computing the pattern
        List<Vector2> direcciones = new();
        Vector2 pos = enemy;
        for (int x = (int)pos.x - 1; x >= 0; x--)
        {
            direcciones.Add(new Vector2(x, pos.y));     // centro
            direcciones.Add(new Vector2(x, pos.y - 1)); // abajo
            direcciones.Add(new Vector2(x, pos.y + 1)); // arriba
        }
        return direcciones.ToArray();
    }
    public Vector2[] Circulo(int area, Vector2 enemy)
    {
        List<Vector2> direcciones = new();
        Vector2 pos = enemy;
        foreach (var dir in circulo) direcciones.Add(pos + dir);
        return direcciones.ToArray();
    }
    public Vector2[] Mortero(Vector2 player)
    {
        List<Vector2> direcciones = new();
        Vector2 playerPos = player;
        System.Random rand = new();
        for (int i = 0; i < 4; i++)
        {
            int x = rand.Next((int)playerPos.x - 2, (int)playerPos.x + 2);
            int y = rand.Next((int)playerPos.y - 3, (int)playerPos.y + 3);
            direcciones.Add(new(x, y));
            direcciones.Add(new(x + 1, y));
            direcciones.Add(new(x, y + 1));
            direcciones.Add(new(x + 1, y + 1));
        }
        return direcciones.ToArray();
    }
    public Vector2[] Golem(Vector2 player)
    {
        List<Vector2> direcciones = new();
        Vector2 playerPos = player;
        Vector2[] direccionesA = { new(0, 0), new(1, 0), new(1, 1), new(1, -1), new(-1, 0), new(-1, 1), new(-1, -1), new(0, 1), new(0, -1) };
        foreach (var dir in direccionesA)
        {
            direcciones.Add(dir + playerPos);
        }
        return direcciones.ToArray();
    }
    public Vector2[] Misilero(int area, Vector2 enemy)
    {
        List<Vector2> direcciones = new();
        Vector2 enemyPos = enemy;
        direcciones.Add(enemyPos + new Vector2(-1, 0));
        direcciones.Add(enemyPos + new Vector2(-2,0));
        direcciones.Add(enemyPos + new Vector2(-3, 0));
        int c = 1;
        for (int i = (int)enemyPos.x - 4; i >= 0; i--)
        {
            direcciones.Add(new Vector2(i, enemyPos.y));
            direcciones.Add(new Vector2(i, enemyPos.y + c));
            direcciones.Add(new Vector2(i, enemyPos.y -c));
            c++;
        }
        return direcciones.ToArray();
    }
    private static Vector2[] Cruz(int area, GameObject enemy)
    {
        var direcciones = new List<Vector2>();
        Tile t = enemy.GetComponent<EnemyController>().GetPos();
        int ancho = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
        int alto = GameObject.Find("GridManager").GetComponent<GridManager>().GetHeight();

        for (int i = 1; i <= area; i++)
        {
            int y = t.y + i;
            if (y >= alto) break;
            direcciones.Add(new Vector2(t.x, y));
        }
        for (int i = 1; i <= area; i++)
        {
            int y = t.y - i;
            if (y < 0) break;
            direcciones.Add(new Vector2(t.x, y));
        }
        for (int i = 1; i <= area; i++)
        {
            int x = t.x + i;
            if (x >= ancho) break;
            direcciones.Add(new Vector2(x, t.y));
        }
        for (int i = 1; i <= area; i++)
        {
            int x = t.x - i;
            if (x < 0) break;
            direcciones.Add(new Vector2(x, t.y));
        }
        return direcciones.ToArray();
        }
    private static Vector2[] TodoElTablero()
    {
        var direcciones = new List<Vector2>();
        int ancho = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
        int alto = GameObject.Find("GridManager").GetComponent<GridManager>().GetHeight();
        for(int i = 0; i < ancho; i++)
        {
            for(int j = 0; j < alto; j++)
            {
                direcciones.Add(new Vector2(i, j));
            }
        }
        return direcciones.ToArray();
    }

}
