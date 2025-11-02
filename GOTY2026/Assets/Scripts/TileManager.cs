using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Xml.Schema;
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
            case "RectaNP":
                tile.gameObject.SendMessage("HighlightDaño");
                direcciones = Recta(GameManager.carta.GetComponent<DisplayCard>().GetArea(),tile);
                direccionesAnt = new Vector2[direcciones.Length];
                c = 0;
                bool seguir = true;
                foreach (var dir in direcciones)
                {
                    if (seguir) {
                        direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                        if (GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                        {
                            if (tile2.ocupado == true)
                            {
                                seguir = false;
                            }
                            tile2.gameObject.SendMessage("HighlightDaño");
                        }
                    }
                    c++;
                }
                break;
                case "Recta":
                direcciones = Recta(GameManager.carta.GetComponent<DisplayCard>().GetArea(),tile);
                direccionesAnt = new Vector2[direcciones.Length];
                c = 0;
                foreach (var dir in direcciones)
                {
                    direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                    if (GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                    {
                        if (tile2.ocupado == true)
                        {
                            seguir = false;
                        }
                        tile2.gameObject.SendMessage("HighlightDaño");
                    }
                    c++;
                }
                break;
                case "TresDirNP":
                var direccionesA = TresDir(GameManager.carta.GetComponent<DisplayCard>().GetArea(),tile);
                int total = 0;
                for (int i = 0; i < 3; i++)
                {
                    total += direccionesA[i].Length;
                }
                direccionesAnt = new Vector2[total];
                c = 0;
                seguir = true;
                for (int i = 0; i< 3; i++) {
                    seguir = true;
                    foreach (var dir in direccionesA[i])
                    {
                        if (seguir)
                        {
                            direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                            if (GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                            {
                                if (tile2.ocupado == true)
                                {
                                    seguir = false;
                                }
                                tile2.gameObject.SendMessage("HighlightDaño");
                            }
                        }
                        c++;
                    }
                }
                break;
                case "Rectangulo":
                direcciones = Rectangulo(GameManager.carta.GetComponent<DisplayCard>().GetArea(),GameManager.carta.GetComponent<DisplayCard>().GetArea2(),tile);
                direccionesAnt = new Vector2[direcciones.Length];
                c = 0;
                foreach (var dir in direcciones)
                {
                    direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                    if (GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                    {
                        tile2.gameObject.SendMessage("HighlightDaño");
                    }
                    c++;
                }
                break;
            default:
                break;
        }
    }
    public void UnHighlightPatron(Tile tile)
    {
        foreach (var dir in direccionesAnt)
        {
            if (GridManager._tiles.TryGetValue(dir, out Tile tile2))
                {
                    tile2.gameObject.SendMessage("UnHighlightDaño");
                }
            }
    }
    public Vector2[] GetDireccionesAnt() => direccionesAnt;
    private static Vector2[] Cruz(int area)
    {
        Vector2[] direcciones = new Vector2[area * 4 + 1];
        direcciones[0] = new Vector2(0, 0);
        for (int i = 1; i <= area; i++)
        {
            direcciones[0 + ((i-1) * 4)] = new Vector2(-1, 0);
            direcciones[1 + ((i-1) * 4)] = new Vector2(1, 0);
            direcciones[2 + ((i-1)* 4)] = new Vector2(0, -1);
            direcciones[3 + ((i-1) * 4)] = new Vector2(0, 1);
        }
        return direcciones;
    }
    private static Vector2[] Recta(int area,Tile tile)
    {
        Vector2[] direcciones = new Vector2[area + 1];
        Tile t = GameManager.player.GetComponent<PlayerController>().GetPos();
        Vector2 direccion = new(tile.x -t.x,tile.y -t.y);
        Vector2 origen = new(0, 0);
        direcciones[0] = origen;
        for (int i = 1; i < area; i++)
        {
            direcciones[i] = origen + (direccion * (i));
        }
        return direcciones;
    }
    private static Vector2[] Rectangulo(int area,int area2,Tile tile)
    {
        Vector2[] direcciones = new Vector2[area * area2];
        Tile t = GameManager.player.GetComponent<PlayerController>().GetPos();
        Vector2 direccion = new(tile.x -t.x,tile.y -t.y);
        int areaD2 = area2 / 2;
        int index = 0;
        for (int i = 0; i < area2; i++)
        {
            for (int j = -areaD2; j <= areaD2; j++)
            {
                if (direccion == Vector2.up)
                    direcciones[index++] = new Vector2(j, i);
                else if (direccion == Vector2.down)
                    direcciones[index++] = new Vector2(-j, -i);
                else if (direccion == Vector2.left)
                    direcciones[index++] = new Vector2(-i, -j);
                else if (direccion == Vector2.right)
                    direcciones[index++] = new Vector2(i, j);
            }
        }
        return direcciones;
    }
    private static Vector2[][] TresDir(int area, Tile tile)
    {
        Vector2[][] direcciones = new Vector2[3][];
        direcciones[0] = new Vector2[area + 1]; // hacia adelante
        direcciones[1] = new Vector2[area]; // lateral 1
        direcciones[2] = new Vector2[area]; // lateral 2
        Vector2 origen = new(0, 0);
        Tile t = GameManager.player.GetComponent<PlayerController>().GetPos();
        Vector2 direccion = new(tile.x - t.x, tile.y - t.y);
        for (int i = 0; i < area; i++)
        {
            if (i == 0)
            {
                direcciones[0][i] = origen;
                direcciones[0][i + 1] = origen + (direccion * (i + 1));  
            }
            else
            {
                direcciones[0][i + 1] = origen + (direccion * (i + 1));
            }
            if (direccion.x != 0)
            {
                direcciones[1][i] = origen + (direccion * (i + 1) + new Vector2(0, -i - 1));
                direcciones[2][i] = origen + (direccion * (i + 1) + new Vector2(0, i + 1));
            }
            else
            {
                direcciones[1][i] = origen + (direccion * (i + 1) + new Vector2(-i - 1, 0));
                direcciones[2][i] = origen + (direccion * (i + 1) + new Vector2(i + 1, 0));
            }
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
        Vector2 pos = new Vector2(centro.x, centro.y);
        for (int x = -rango; x <= rango; x++)
        {
            for (int y = -rango; y <= rango; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= rango)
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
