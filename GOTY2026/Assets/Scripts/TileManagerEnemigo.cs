using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManagerEnemigo : MonoBehaviour
{
    public Vector2[] direccionesAnt;
    private String lastPatron;
    public Vector2[] GetRango() => direccionesAnt;
    public Boolean visto = false;
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
                direccionesAnt = RectaHorizontal(enemy.GetComponent<DisplayEnemy>().GetArea(), enemy);
                break;
            case "Caballero":
                if (TurnManager.numTurno == 0)
                {
                    direccionesAnt = Caballero1(enemy.GetComponent<DisplayEnemy>().GetArea(), enemy);
                }
                else
                {
                    direccionesAnt = Circulo(enemy.GetComponent<DisplayEnemy>().GetArea(), enemy);
                }
                break;
            case "Sniper":
                Sniper(enemy.GetComponent<DisplayEnemy>().GetArea(), enemy);
                break;
            case "Mortero":
                direccionesAnt = Mortero(enemy.GetComponent<DisplayEnemy>().GetArea(), enemy);
                break;
            case "Golem":
                direccionesAnt = Golem();
                break;
            case "Misilero":
                direccionesAnt = Misilero(enemy.GetComponent<DisplayEnemy>().GetArea(), enemy);
                break;
            default:
                break;
        }
    }
    public void HighlightEnemyTiles(GameObject enemy)
    {
        foreach (Vector2 dir in direccionesAnt)
        {
            if (GridManager._tiles.TryGetValue(dir, out Tile tile2))
                tile2.gameObject.SendMessage("HighlightEnemy");
        }
    }
    public void UnHighlightEnemyTiles()
    {
        foreach (var dir in direccionesAnt)
        {
            if (GridManager._tiles.TryGetValue(dir, out Tile tile2))
            {
                tile2.gameObject.SendMessage("UnHighlightEnemy");
            }
        }
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
    /*Para el robot, ya que ataca solo en su fila*/
    private static Vector2[] RectaHorizontal(int area, GameObject enemy) // tile la casilla seleccionada
    {

        List<Vector2> direcciones = new();
        Tile t = enemy.GetComponent<EnemyController>().GetPos(); //t = pos enemigo
        int playerx = GameManager.player.GetComponent<PlayerController>().GetPos().x; // para atacar hacia el enemigo
        int enemyx = enemy.GetComponent<EnemyController>().GetPos().x;
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
                direcciones.Add(new(i, t.y));
                cont++;
                if (cont == area) break;
            }
        }
        else
        {
            for (int i = enemyx - 1; i < x && i >= 0; i--)
            {
                direcciones.Add(new(i, t.y));
                cont++;
                if (cont == area) break;
            }
        }
        return direcciones.ToArray();
    }
    private static Vector2[] Caballero1(int area, GameObject enemy)
    {
        int largo = GameObject.Find("GridManager").GetComponent<GridManager>().GetWidth();
        List<Vector2> direcciones = new();
        Vector2 pos = new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y);
        for (int x = (int)pos.x - 1; x >= 0; x--)
        {
            direcciones.Add(new Vector2(x, pos.y));     // centro
            direcciones.Add(new Vector2(x, pos.y - 1)); // abajo
            direcciones.Add(new Vector2(x, pos.y + 1)); // arriba
        }
        return direcciones.ToArray();
    }
    private Vector2[] Circulo(int area, GameObject enemy)
    {
        List<Vector2> direcciones = new();
        Vector2 pos = new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y);
        foreach (var dir in circulo) direcciones.Add(pos + dir);
        return direcciones.ToArray();
    }
    public void Sniper(int area, GameObject enemy)
    {
        GameObject.Find("Player").GetComponent<PlayerController>().Mirilla();
    }
    public Vector2[] Mortero(int area, GameObject enemy)
    {
        List<Vector2> direcciones = new();
        Vector2 playerPos = new(GameObject.Find("Player").GetComponent<PlayerController>().GetPos().x, GameObject.Find("Player").GetComponent<PlayerController>().GetPos().y);
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
    public Vector2[] Golem()
    {
        List<Vector2> direcciones = new();
        Vector2 playerPos = new(GameObject.Find("Player").GetComponent<PlayerController>().GetPos().x, GameObject.Find("Player").GetComponent<PlayerController>().GetPos().y);
        Vector2[] direccionesA = { new(0, 0), new(1, 0), new(1, 1), new(1, -1), new(-1, 0), new(-1, 1), new(-1, -1), new(0, 1), new(0, -1) };
        foreach (var dir in direccionesA)
        {
            direcciones.Add(dir + playerPos);
        }
        return direcciones.ToArray();
    }
    public Vector2[] Misilero(int area, GameObject enemy)
    {
        List<Vector2> direcciones = new();
        Vector2 enemyPos = new(enemy.GetComponent<EnemyController>().GetPos().x, enemy.GetComponent<EnemyController>().GetPos().y);
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
    
    
}
