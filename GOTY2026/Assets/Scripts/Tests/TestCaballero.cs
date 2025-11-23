using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class TestCaballero
{
    // Prepara GridManager, GameManager.enemyList y GameObjects necesarios para que el test funcione
    [Test]
    public void TestCaballeroSimplePasses()
    {
        // --- GridManager + tiles ---
        GameObject gridGO = new GameObject("GridManager");
        GridManager grid = gridGO.AddComponent<GridManager>();
        // ajustar dimensiones acorde al test (x: 0..7, y: 0..3)
        typeof(GridManager).GetField("_width", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).SetValue(grid, 8);
        typeof(GridManager).GetField("_height", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).SetValue(grid, 4);

        GridManager._tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                GameObject tileGO = new GameObject($"Tile {x} {y}");
                Tile tile = tileGO.AddComponent<Tile>();
                tile.x = x;
                tile.y = y;
                tileGO.transform.position = new Vector3(x, y, 0);
                GridManager._tiles[new Vector2(x, y)] = tile;
            }
        }

        // --- GameManager enemyList (creamos un Enemy ScriptableObject con id 3 y patron 'Caballero') ---
        Enemy enemySO = ScriptableObject.CreateInstance<Enemy>();
        enemySO.id = 3;
        enemySO.patronAtaque = "Caballero";
        // asignar lista estática
        GameManager.enemyList = new List<Enemy> { enemySO };

        // --- Crear objeto Player (para que EnemyController.Awake lo encuentre) ---
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<PlayerController>();

        // --- Crear objeto enemigo (display + sprite) ---
        GameObject go = new GameObject("TestEnemy");
        // Crear HealthBar hijo con BarraVidaEnemy y componentes UI mínimos
        GameObject healthBar = new GameObject("HealthBar");
        healthBar.transform.SetParent(go.transform);
        BarraVidaEnemy barra = healthBar.AddComponent<BarraVidaEnemy>();

        // Crear Slider y Text y asignarlos a los campos privados vía reflexión antes de añadir EnemyController
        GameObject sliderGO = new GameObject("Slider");
        sliderGO.transform.SetParent(healthBar.transform);
        UnityEngine.UI.Slider slider = sliderGO.AddComponent<UnityEngine.UI.Slider>();
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(healthBar.transform);
        UnityEngine.UI.Text texto = textGO.AddComponent<UnityEngine.UI.Text>();

        var barraType = typeof(BarraVidaEnemy);
        var sliderField = barraType.GetField("sliderVidaEnemy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var textoField = barraType.GetField("textoVida", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (sliderField != null) sliderField.SetValue(barra, slider);
        if (textoField != null) textoField.SetValue(barra, texto);

        // DisplayEnemy espera un SpriteRenderer
        go.AddComponent<SpriteRenderer>();
        DisplayEnemy de = go.AddComponent<DisplayEnemy>();
        // Ahora ActualizarID encontrará el enemy en GameManager.enemyList
        de.ActualizarID(3);

        // Añadir TileManagerEnemigo y finalmente EnemyController (EnemyController.Awake usará la Barra y el Player ya creados)
        TileManagerEnemigo tme = go.AddComponent<TileManagerEnemigo>();
        EnemyController cab = go.AddComponent<EnemyController>();

        // Mover al enemigo a (7,2)
        cab.Mover(new Vector2(7, 2));

        Vector2[] direccionesBien = {new Vector2(6,2),new Vector2(6,1),new Vector2(6,3),new Vector2(5,2),new Vector2(5,1),new Vector2(5,3),new Vector2(4,2),new Vector2(4,1),new Vector2(4,3),
        new Vector2(3,2),new Vector2(3,1),new Vector2(3,3),new Vector2(2,2),new Vector2(2,1),new Vector2(2,3),new Vector2(1,2),new Vector2(1,1),new Vector2(1,3),new Vector2(0,2),new Vector2(0,1),new Vector2(0,3)};

        tme.CalculoTiles(go);
        Vector2[] direccionesTest = tme.GetRango();
        CollectionAssert.AreEquivalent(direccionesBien, direccionesTest);

        // Ahora probar la rama Circulo (numTurno = 1)
        TurnManager.numTurno = 1;
        cab.Mover(new Vector2(0, 2));
        Vector2[] direccionesBien2 = { new Vector2(-1, 2),new Vector2(-1, 1),new Vector2(-1, 3),new Vector2(1, 2),new Vector2(1, 1),new Vector2(1, 3),new Vector2(0, 3),new Vector2(0, 1)};
        tme.CalculoTiles(go);
        Vector2[] direccionesTest2 = tme.GetRango();
        CollectionAssert.AreEquivalent(direccionesBien2, direccionesTest2);

        // Limpieza
        Object.DestroyImmediate(go);
        Object.DestroyImmediate(gridGO);
        foreach (var t in GridManager._tiles.Values)
        {
            if (t != null) Object.DestroyImmediate(t.gameObject);
        }
        GridManager._tiles = null;
    }
}
