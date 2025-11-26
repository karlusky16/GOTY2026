using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class TestGolem
{
    // Prepara GridManager, GameManager.enemyList y GameObjects necesarios para que el test funcione
    [Test]
    public void TestGolemA()
    {
        // Crear player y asignar posicion (Tile)
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        PlayerController pc = player.AddComponent<PlayerController>();
        GameObject tileGO = new GameObject("TilePlayer");
        Tile tile = tileGO.AddComponent<Tile>();
        tile.x = 3;
        tile.y = 2;
        pc.posicion = tile;

        // Crear Enemy ScriptableObject id=5 con patron Golem
        Enemy enemySO = ScriptableObject.CreateInstance<Enemy>();
        enemySO.id = 5;
        enemySO.patronAtaque = "Golem";
        GameManager.enemyList = new List<Enemy> { enemySO };

        // Crear objeto enemigo y DisplayEnemy que referencie al SO
        GameObject go = new GameObject("TestEnemy");
        go.AddComponent<SpriteRenderer>();
        DisplayEnemy de = go.AddComponent<DisplayEnemy>();
        de.ActualizarID(5);

        // Añadir TileManagerEnemigo y llamar Golem
        TileManagerEnemigo tme = go.AddComponent<TileManagerEnemigo>();
        Vector2[] resultados = tme.Golem();

        // Deben ser 9 posiciones (centro + 8 adyacentes)
        Assert.AreEqual(9, resultados.Length);

        // Construir conjunto esperado alrededor del player
        Vector2[] esperadas = {
            new Vector2(3,2),
            new Vector2(4,2),
            new Vector2(4,3),
            new Vector2(4,1),
            new Vector2(2,2),
            new Vector2(2,3),
            new Vector2(2,1),
            new Vector2(3,3),
            new Vector2(3,1)
        };

        CollectionAssert.AreEquivalent(esperadas, resultados);

        // Limpieza
        Object.DestroyImmediate(go);
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(tileGO);
    }
}