using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int _width, _height;

    public Tile _tilePrefab;
    public Transform parent;
    public Transform _cam;
    private Boolean pulso;
    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
        pulso = false;
        
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, parent);
                
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

<<<<<<< Updated upstream
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
=======
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);//coloca la c�mara.
>>>>>>> Stashed changes
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Activar(pulso);
        }
    }


    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
    public void Activar(Boolean pulso)
    {
        gameObject.SetActive(pulso);
        pulso = !pulso;
    }

}
