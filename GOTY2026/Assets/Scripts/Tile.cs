using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color mainColor, otherColor;
    private SpriteRenderer render;
    public GameObject _highlight;

    //Variables para obtener la posicion del tile
    public int x, y;

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
        _highlight.SetActive(true);
    }
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
   
}