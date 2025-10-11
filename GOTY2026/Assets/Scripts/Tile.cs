using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mono.Cecil.Cil;
using UnityEditor.EditorTools;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color mainColor, otherColor;
    private SpriteRenderer render;
    public GameObject _highlight;
    public GameObject gridManager;
    //Variables para obtener la posicion del tile
    public int x, y;

    void Start() {
    }
    public void Init(bool isOffset)
    {
        Debug.Log(mainColor);
        render = GetComponent<SpriteRenderer>();
        if (isOffset)
        {
            render.color = Color.black;
        }

        else
        {
            render.color = Color.red;
        }
    }

    void OnMouseEnter()
    {
        if (Player.cartaSeleccionada == true)
        {
            if (Player.carta.GetComponent<DisplayCard>().patron == "Cruz")
            {
                Highlight();
                Tile tile = null;
                Boolean leido = GridManager._tiles.TryGetValue(new Vector2(x-1, y),out tile);
                if (leido == true)
                {
                    tile.Highlight();
                }
                leido = GridManager._tiles.TryGetValue(new Vector2(x + 1, y),out tile);
                if (leido == true)
                {
                    tile.Highlight();
                }
                leido = GridManager._tiles.TryGetValue(new Vector2(x, y - 1),out tile);
                if (leido == true)
                {
                    tile.Highlight();
                }
                leido = GridManager._tiles.TryGetValue(new Vector2(x, y + 1),out tile);
                if (leido == true)
                {
                    tile.Highlight();
                }
            }
        }
    }
    void OnMouseExit()
    {
        if (Player.carta != null && Player.carta.GetComponent<DisplayCard>().patron == "Cruz")
        {
            UnHighlight();
            Tile tile ;
            Boolean leido = GridManager._tiles.TryGetValue(new Vector2(x-1, y),out tile);
            if (leido == true)
            {
                tile.UnHighlight();
            }
            leido = GridManager._tiles.TryGetValue(new Vector2(x+1, y),out tile);
            if (leido == true)
            {
                tile.UnHighlight();
            }
            leido = GridManager._tiles.TryGetValue(new Vector2(x, y - 1),out tile);
            if (leido == true)
            {
                tile.UnHighlight();
            }
            leido = GridManager._tiles.TryGetValue(new Vector2(x, y + 1),out tile);
            if (leido == true)
            {
                tile.UnHighlight();
            }
        }
    }

    void Highlight()
    {
        _highlight.SetActive(true);
    }
    
     void UnHighlight() {
        _highlight.SetActive(false);
    }
   
}