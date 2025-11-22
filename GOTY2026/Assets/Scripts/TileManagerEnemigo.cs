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
            default:
                break;
        }
    }
    public void HighlightEnemyTiles(GameObject enemy)
    {
        foreach (Vector2 dir in direccionesAnt){
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
        if(enemyx < 0 || enemyx >= x) {Debug.Log("Posición del enemy en RectaHorizontal en TileMEnemigo"); return null;}
        if(playerx < 0 || playerx >= x) {Debug.Log("Posición del player en RectaHorizontal en TileMEnemigo"); return null;}

        if(playerx - enemyx > 0) plaDer = true;
        int cont = 0;

        if (plaDer)
        {
            for(int i = enemyx + 1; i < x && i >= 0; i++)
            {
                direcciones.Add(new(i, t.y));
                cont ++;
                if(cont == area) break;
            }
        }
        else
        {
            for(int i = enemyx - 1; i < x && i >= 0; i--)
            {
                direcciones.Add(new(i, t.y));
                cont ++;
                if(cont == area) break;
            }
        }
        return direcciones.ToArray();
    }
    
}
