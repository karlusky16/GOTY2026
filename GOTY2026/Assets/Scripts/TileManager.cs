using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Xml.Schema;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private String lastPatron;
    private Vector2[] direccionesAnt;
    public List<Tile> tilesEnRango;
    public Tile tileMov;//Calculad en el patrón la Tile a la que moverse y guardarla en esta variable
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Tile> GetRango() => tilesEnRango;
    public Tile GetTileMov() => tileMov; 
    //Incluye patrones de daño y de movimiento, en los de movimiento se DEBE modificar la variable tileMov para saber a que tile moverse
    public void HighlightPatron(Tile tile)
    {
        lastPatron = GameManager.carta.GetComponent<DisplayCard>().GetPatron();
        switch (lastPatron)
        {
            case "Cruz"://tipo Daño
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
            case "RectaNP"://Tipo Daño
                direcciones = Recta(GameManager.carta.GetComponent<DisplayCard>().GetArea(), tile);
                direccionesAnt = new Vector2[direcciones.Length];
                c = 0;
                bool seguir = true;
                foreach (var dir in direcciones)
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
                break;
            case "Recta"://Tipo Daño
                direcciones = Recta(GameManager.carta.GetComponent<DisplayCard>().GetArea(), tile);
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
            case "RectaM"://Tipo Movimiento + Daño
                direcciones = Recta(GameManager.carta.GetComponent<DisplayCard>().GetArea(), tile);
                direccionesAnt = new Vector2[direcciones.Length];
                c = 0;
                Boolean si = true;
                foreach (var dir in direcciones)
                {
                    direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                    if (si = GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                    {
                        tile2.gameObject.SendMessage("HighlightDaño");
                    }
                    if (si)
                    {
                        tileMov = tile2;
                    }
                    c++;
                }
                break;
            case "TresDirNP"://Tipo Daño
                var direccionesA = TresDir(GameManager.carta.GetComponent<DisplayCard>().GetArea(), tile);
                int total = 0;
                for (int i = 0; i < 3; i++)
                {
                    total += direccionesA[i].Length;
                }
                direccionesAnt = new Vector2[total];
                c = 0;
                seguir = true;
                for (int i = 0; i < 3; i++)
                {
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
            case "Rectangulo"://Tipo Daño
                direcciones = Rectangulo(GameManager.carta.GetComponent<DisplayCard>().GetArea(), GameManager.carta.GetComponent<DisplayCard>().GetArea2(), tile);
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
            case "Rectangulo2":
                direcciones = Rectangulo2(GameManager.carta.GetComponent<DisplayCard>().GetArea(), GameManager.carta.GetComponent<DisplayCard>().GetArea2(), tile);
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
            case "Tile":
                direccionesAnt = new Vector2[1];
                direccionesAnt[0] = new Vector2(tile.x, tile.y);
                if (GameManager.carta.GetComponent<DisplayCard>().GetTipo() == 1 ||GameManager.carta.GetComponent<DisplayCard>().GetTipo() == 2 )
                {
                    tileMov = tile;
                }
                tile.gameObject.SendMessage("HighlightDaño");
                break;
            case "RectaH":
                direcciones = RectaH(GameManager.carta.GetComponent<DisplayCard>().GetArea(), tile);
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
            /*case "RectaC":
                direcciones = Recta(GameManager.carta.GetComponent<DisplayCard>().GetArea(), tile);
                direccionesAnt = new Vector2[direcciones.Length];
                c = 0;
                int seguirC = 0;
                foreach (var dir in direcciones)
                {
                    if (seguirC < 2)
                    {
                        direccionesAnt[c] = new Vector2(tile.x, tile.y) + dir;
                        if (GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + dir, out Tile tile2))
                        {
                            if (tile2.ocupado == true)
                            {
                                seguirC++;
                            }
                            if (GameManager.carta.GetComponent<DisplayCard>().GetTipo() == 1 ||GameManager.carta.GetComponent<DisplayCard>().GetTipo() == 2)
                                tileMov = tile2;   
                            tile2.gameObject.SendMessage("HighlightDaño");
                        }
                    }
                    c++;
                }
                break;*/
            default:
                break;
        }
    }
    public void UnHighlightPatron(Tile tile)
    {
        //Aqui se manda UnHighlight a todas las tiles en direccionesAnt, por lo tanto se debe haber calculado antes
        foreach (var dir in direccionesAnt)
        {
            if (GridManager._tiles.TryGetValue(dir, out Tile tile2))
            {
                tile2.gameObject.SendMessage("UnHighlightDaño");
            }
        }
    }
    //Getter direcionesAnt
    public Vector2[] GetDireccionesAnt() => direccionesAnt;
    //Calculo de patrones
    private static Vector2[] Cruz(int area)
    {
        Vector2[] direcciones = new Vector2[area * 4 + 1];
        direcciones[0] = new Vector2(0, 0);
        for (int i = 1; i <= area; i++)
        {
            direcciones[0 + ((i - 1) * 4)] = new Vector2(-1, 0);
            direcciones[1 + ((i - 1) * 4)] = new Vector2(1, 0);
            direcciones[2 + ((i - 1) * 4)] = new Vector2(0, -1);
            direcciones[3 + ((i - 1) * 4)] = new Vector2(0, 1);
        }
        return direcciones;
    }
    private static Vector2[] Recta(int area,Tile tile)
    {
        List<Vector2> direcciones = new();
        Tile t = GameManager.player.GetComponent<PlayerController>().GetPos();
        Vector2 direccion = new(tile.x -t.x,tile.y -t.y);
        Vector2 origen = new(0, 0);
        direcciones.Add(origen);
        for (int i = 1; i < area; i++)
        {
            direcciones.Add(direccion * (i));
        }
        if (GameManager.carta.GetComponent<DisplayCard>().GetTipo() == 1 ||GameManager.carta.GetComponent<DisplayCard>().GetTipo() == 2)
        {
            GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + direcciones[^1], out Tile tile2);
            while (tile2 != null && tile2.ocupado && direcciones.Count > 0)
            {
                direcciones.RemoveAt(direcciones.Count - 1);
                GridManager._tiles.TryGetValue(new Vector2(tile.x, tile.y) + direcciones[^1], out tile2);
            }
        }
        return direcciones.ToArray();
    }
    private static Vector2[] RectaH(int area,Tile tile)
    {
        List<Vector2> direcciones = new();
        Tile t = GameManager.player.GetComponent<PlayerController>().GetPos();
        Vector2 origen = new(0, 0);
        direcciones.Add(origen);
        for (int i = 1; i < area / 2; i++)
        {
            direcciones.Add(origen + new Vector2(0, i));
            direcciones.Add(origen + new Vector2(0,-i));
        }
        return direcciones.ToArray();
    }
    
    private static Vector2[] Rectangulo(int area, int area2, Tile tile)
    {
        // area = number of columns, area2 = number of rows
        Tile t = GameManager.player.GetComponent<PlayerController>().GetPos();
        Vector2 direccion = new(tile.x - t.x, tile.y - t.y);
        int areaD2 = area2 / 2;
        Vector2[] direcciones = new Vector2[area * area2];
        int index = 0;
        for (int i = 0; i < area; i++)
        {
            int offsetX = i;
            for (int j = 0; j < area2; j++)
            {
                int offsetY = j - areaD2;
                if (direccion == Vector2.up)
                    direcciones[index++] = new Vector2(offsetY, offsetX);
                else if (direccion == Vector2.down)
                    direcciones[index++] = new Vector2(-offsetY, -offsetX);
                else if (direccion == Vector2.left)
                    direcciones[index++] = new Vector2(-offsetX, -offsetY);
                else if (direccion == Vector2.right)
                    direcciones[index++] = new Vector2(offsetX, offsetY);
            }
        }
        return direcciones;
    }
    private static Vector2[] Rectangulo2(int area, int area2, Tile tile)
    {
        // Generate a centered rectangle of size (area x area2) around the tile
        // Returns relative offsets (to be added to the tile position by the caller)
        Vector2[] direcciones = new Vector2[area * area2];
        int c = 0;
        int halfX = area / 2;
        int halfY = area2 / 2;
        for (int i = 0; i < area; i++)
        {
            int offsetX = i - halfX;
            for (int j = 0; j < area2; j++)
            {
                int offsetY = j - halfY;
                direcciones[c++] = new Vector2(offsetX, offsetY);
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
    //Funcion Marcar Rango
    public void MarcarRango(Tile tile)
    {
        int rango = GameManager.carta.GetComponent<DisplayCard>().GetRango();
        tilesEnRango = ObtenerTilesEnRango(tile, rango);
        foreach (Tile t in tilesEnRango)
        {
            t.gameObject.SendMessage("Highlight");
        }

    }
    //Funcion Desmarcar Rango
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
    //Funcion Obtener Tiles en Rango
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
}
