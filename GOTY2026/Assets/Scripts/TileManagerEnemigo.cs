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
}
