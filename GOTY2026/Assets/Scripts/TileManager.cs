using System;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private String lastPatron;
    private Vector2[] direccionesAnt;
    public List<Tile> tilesEnRango;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Tile> GetRango() => tilesEnRango;
    public void HighlightPatron(Tile tile)
    {
        lastPatron = GameManager.carta.GetComponent<DisplayCard>().GetPatron();
        switch (lastPatron)
        {
            case "Cruz":
                tile.gameObject.SendMessage("HighlightDaño");
                var direcciones = Cruz(GameManager.carta.GetComponent<DisplayCard>().GetArea());
                direccionesAnt = new Vector2[direcciones.Length];
                int c = 0;
                foreach (var dir in direcciones)
                {
                    direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                    if (GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                        tile2.gameObject.SendMessage("HighlightDaño");
                    c++;
                }
                break;
            default:
                break;
        }
    }
    public void UnHighlightPatron(Tile tile)
    {
        lastPatron = GameManager.carta.GetComponent<DisplayCard>().GetPatron();
        switch (lastPatron)
        {
            case "Cruz":
                tile.gameObject.SendMessage("UnHighlightDaño");
                var direcciones = Cruz(GameManager.carta.GetComponent<DisplayCard>().GetArea());
                direccionesAnt = new Vector2[direcciones.Length + 1];
                direccionesAnt[0] = new Vector2(tile.x,tile.y);
                int c = 1;
                foreach (var dir in direcciones)
                {
                    direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                    if (GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                        tile2.gameObject.SendMessage("UnHighlightDaño");
                    c++;
                }
                break;
            default:
                break;
        }
    }
    public Vector2[] GetDireccionesAnt() => direccionesAnt;
    private static Vector2[] Cruz(int area)
    {
        Vector2[] direcciones = new Vector2[area * 4];
        for (int i = 0; i < area; i++)
        {
            direcciones[0 + (i * 4)] = new Vector2(-1, 0);
            direcciones[1 + (i * 4)] = new Vector2(1, 0);
            direcciones[2 + (i * 4)] = new Vector2(0, -1);
            direcciones[3 + (i * 4)] = new Vector2(0, 1);
        }
        return direcciones;
    }
    public void MarcarRango(Tile tile)
    {
        int rango = GameManager.carta.GetComponent<DisplayCard>().GetRango();
        tilesEnRango = ObtenerTilesEnRango(tile, rango);
        foreach (Tile t in tilesEnRango)
        {
            t.gameObject.SendMessage("Highlight");
        }

    }
    public void DesmarcarRango(Tile tile)
    {
        int rango = GameManager.carta.GetComponent<DisplayCard>().GetRango();
        tile.gameObject.SendMessage("UnHighlight");
        tilesEnRango = ObtenerTilesEnRango(tile, rango);
        foreach (Tile t in tilesEnRango)
        {
            t.gameObject.SendMessage("UnHighlight");
        }

    }
    
    List<Tile> ObtenerTilesEnRango(Tile centro, int rango)
    {
        List<Tile> resultado = new List<Tile>();
        resultado.Add(centro);
        Vector2 pos = new Vector2(centro.x, centro.y);
        for (int x = -rango; x <= rango; x++)
        {
            for (int y = -rango; y <= rango; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= rango) // distancia Manhattan
                {
                    GridManager._tiles.TryGetValue(new Vector2(pos.x + x, pos.y + y), out Tile t);
                    if (t != null && t != centro)
                    {
                        resultado.Add(t);
                    }
                }
            }
        }

        return resultado;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
