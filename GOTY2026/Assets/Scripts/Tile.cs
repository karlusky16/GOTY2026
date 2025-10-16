using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color mainColor, otherColor;
    private SpriteRenderer render;
    private String lastPatron;
    public GameObject _highlight;

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
<<<<<<< Updated upstream

=======
        if (Player.cartaSeleccionada == true)
        {
            if (Player.carta.GetComponent<DisplayCard>().patron == "Cruz")
            {
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

    void Highlight()
    {
>>>>>>> Stashed changes
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
   
}