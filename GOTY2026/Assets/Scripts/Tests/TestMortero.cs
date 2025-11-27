using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class TestMortero
{
    // Test directo a Mortero() creando solo el Player y su Tile de posición
    [Test]
    public void TestMorteroPattern()
    {
        // Crear Player minimal
        var playerGO = new GameObject("Player");
        var player = playerGO.AddComponent<PlayerController>();
        var playerTileGO = new GameObject("PlayerTile");
        var playerTile = playerTileGO.AddComponent<Tile>();
        // establecer la posición del jugador en (3,2)
        playerTile.x = 3;
        playerTile.y = 2;
        player.posicion = playerTile;

        // Crear TileManagerEnemigo
        var go = new GameObject("tme");
        var tme = go.AddComponent<TileManagerEnemigo>();

        Vector2[] resultados = tme.Mortero();

        // Mortero genera 4 bloques de 4 tiles -> 16 posiciones
        Assert.AreEqual(16, resultados.Length);

        // Comprobar que todas las posiciones están dentro del rango esperado
        // para player (3,2): x en [1,5], y en [-1,5]
        foreach (var v in resultados)
        {
            Assert.GreaterOrEqual(v.x, 1f);
            Assert.LessOrEqual(v.x, 5f);
            Assert.GreaterOrEqual(v.y, -1f);
            Assert.LessOrEqual(v.y, 5f);
        }

        // Limpieza
        Object.DestroyImmediate(go);
        Object.DestroyImmediate(playerGO);
        Object.DestroyImmediate(playerTileGO);
    }
}