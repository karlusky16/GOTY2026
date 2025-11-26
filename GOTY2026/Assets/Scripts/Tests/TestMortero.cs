using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class TestMortero
{
    // Prepara GridManager, GameManager.enemyList y GameObjects necesarios para que el test funcione
    [Test]
    public void TestMorteroA()
    {
        // Crear player y asignar posicion (Tile)
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        PlayerController pc = player.AddComponent<PlayerController>();
        // crear un Tile para la posicion del player
        GameObject tileGO = new GameObject("TilePlayer");
        Tile tile = tileGO.AddComponent<Tile>();
        tile.x = 4;
        tile.y = 2;
        pc.posicion = tile;

        // Crear Enemy ScriptableObject id=5 con patron Mortero y ponerlo en GameManager.enemyList
        Enemy enemySO = ScriptableObject.CreateInstance<Enemy>();
        enemySO.id = 5;
        enemySO.patronAtaque = "Mortero";
        GameManager.enemyList = new List<Enemy> { enemySO };

        // Crear objeto enemigo y DisplayEnemy que referencie al SO
        GameObject go = new GameObject("TestEnemy");
        go.AddComponent<SpriteRenderer>();
        DisplayEnemy de = go.AddComponent<DisplayEnemy>();
        de.ActualizarID(5);

        // Añadir TileManagerEnemigo para llamar Mortero directamente
        TileManagerEnemigo tme = go.AddComponent<TileManagerEnemigo>();

        // Llamar Mortero (area no es usada aquí)
        Vector2[] resultados = tme.Mortero(1, go);

        // Debe devolver 16 posiciones (4 iteraciones * 4 posiciones)
        Assert.AreEqual(16, resultados.Length);

        // Comprobar que todas las posiciones están dentro del rango esperado respecto al player
        int px = pc.GetPos().x;
        int py = pc.GetPos().y;
        foreach (var v in resultados)
        {
            // Según la implementación, x en [px-2, px+2], y en [py-3, py+3]
            Assert.GreaterOrEqual(v.x, px - 2);
            Assert.LessOrEqual(v.x, px + 2);
            Assert.GreaterOrEqual(v.y, py - 3);
            Assert.LessOrEqual(v.y, py + 3);
        }

        // Limpieza
        Object.DestroyImmediate(go);
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(tileGO);
    }
}