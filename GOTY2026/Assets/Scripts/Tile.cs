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

            Vector2[] direcciones ={new Vector2(-1, 0),new Vector2(1, 0),new Vector2(0, -1),new Vector2(0, 1)};
            foreach (var dir in direcciones)
            {
                if (GridManager._tiles.TryGetValue(new Vector2(x, y) + dir, out Tile tile))
                    tile.Highlight();
                }
            }
        }
    }
    void OnMouseExit()
    {
        if (Player.cartaSeleccionada == true)
        {
            if (Player.carta.GetComponent<DisplayCard>().patron == "Cruz")
        {
            UnHighlight();

            Vector2[] direcciones = { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1) };
            foreach (var dir in direcciones)
            {
                if (GridManager._tiles.TryGetValue(new Vector2(x, y) + dir, out Tile tile))
                    tile.UnHighlight();
            }
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