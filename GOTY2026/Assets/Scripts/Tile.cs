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
    public GameObject _highlightD;
    public GameObject _highlightE;
    public GameObject gridManager;
    public Boolean ocupado = false;
    public GameObject ocupadoObj;
    //Variables para obtener la posicion del tile
    public int x, y;
    private object direccionesEfecto;


    void Start()
    {
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
        Debug.Log("Mouse encima tile");
        if (GameManager.cartaSeleccionada == true && GameObject.Find("GameManager").GetComponent<TileManager>().GetRango().Contains(this))
        {
            GameObject.Find("GameManager").SendMessage("HighlightPatron", this);
        }
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse encima tile");
        if (GameManager.cartaSeleccionada == true && GameObject.Find("GameManager").GetComponent<TileManager>().GetRango().Contains(this))
        {
            GameObject.Find("GameManager").SendMessage("UnHighlightPatron", this);
        }
    }
    //Habria que mover todo lo del Highlight y Testeo de efecto a otro script
    void OnMouseDown()
    {
        if (GameManager.cartaSeleccionada == true && GameObject.Find("GameManager").GetComponent<TileManager>().GetRango().Contains(this))
        {
            GameObject.Find("GameManager").SendMessage("UnHighlightPatron", this);
            GameObject.Find("GameManager").SendMessage("DesmarcarRango", GameManager.player.GetComponent<PlayerController>().GetPos());
            GameManager.carta.GetComponent<CardAction>().Efecto(GameObject.Find("GameManager").GetComponent<TileManager>().GetDireccionesAnt());
        }
    }

    //Los he hecho públicos para poder utilizarlos desde el enemigo
    public void Highlight()
    {
        _highlight.SetActive(true);
    }

    public void UnHighlight()
    {

        _highlight.SetActive(false);
    }
    void HighlightDaño()
    {
        _highlightD.SetActive(true);
    }

    void UnHighlightDaño()
    {

        _highlightD.SetActive(false);
    }
    void HighlightEnemy()
    {
        _highlightE.SetActive(true);
    }
    
    void UnHighlightEnemy() {

        _highlightE.SetActive(false);
    }


}