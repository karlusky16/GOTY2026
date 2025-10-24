using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color mainColor, otherColor;
    private SpriteRenderer render;
    private String lastPatron;
    public GameObject _highlight;
    public GameObject gridManager;
    public Boolean ocupado = false;
    public GameObject ocupadoObj;
    //Variables para obtener la posicion del tile
    public int x, y;
    private object direccionesEfecto;


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
        if (GameManager.cartaSeleccionada) {
            if (GameManager.carta.GetComponent<DisplayCard>().patron == "Cruz") {
                lastPatron = "Cruz";
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
            if (GameManager.cartaSeleccionada == true)
            {
                if (GameManager.carta.GetComponent<DisplayCard>().patron == "Cruz")
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
            else
            {
                UnHighlight();
                if (lastPatron == "Cruz")
                {
                    Vector2[] direcciones = { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1) };
                    foreach (var dir in direcciones)
                    {
                        if (GridManager._tiles.TryGetValue(new Vector2(x, y) + dir, out Tile tile))
                            tile.UnHighlight();
                    }
                }
            }

        }
    //Habria que mover todo lo del Highlight y Testeo de efecto a otro script
    void OnMouseDown()
    {
        if (GameManager.cartaSeleccionada == true)
        {
            if (GameManager.carta.GetComponent<DisplayCard>().patron == "Cruz")
            {
                UnHighlight();
                int i = 0;
                Vector2[] direccionesEfecto = new Vector2[5];
                Vector2[] direcciones = { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1) };
                foreach (var dir in direcciones)
                {
                    if (GridManager._tiles.TryGetValue(new Vector2(x, y) + dir, out Tile tile))
                    {
                        tile.UnHighlight();
                        direccionesEfecto[i] = (new Vector2(x, y) + dir);
                        i++;
                    }
                }
                direccionesEfecto[4] = (new Vector2(x, y));
                GameManager.carta.GetComponent<CardAction>().Efecto(direccionesEfecto);
            }
        }
    }

    //Los he hecho públicos para poder utilizarlos desde el enemigo
    public void Highlight()
    {
        _highlight.SetActive(true);
    }
    
    public void UnHighlight() {

        _highlight.SetActive(false);
    }


}