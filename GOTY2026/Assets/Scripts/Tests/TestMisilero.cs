using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestMisilero
{
    [Test]
    public void TestMisileroPattern()
    {
        // Preparar Player con Tile en (2,2)
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        PlayerController pc = player.AddComponent<PlayerController>();
        GameObject playerTileGO = new GameObject("PlayerTile");
        Tile playerTile = playerTileGO.AddComponent<Tile>();
        playerTile.x = 2;
        playerTile.y = 2;
        pc.posicion = playerTile;

        // Preparar Enemy ScriptableObject id=8 con patron Misilero
        Enemy enemySO = ScriptableObject.CreateInstance<Enemy>();
        enemySO.id = 8;
        enemySO.patronAtaque = "Misilero";
        GameManager.enemyList = new List<Enemy> { enemySO };

        // Crear objeto enemigo y DisplayEnemy que referencie al SO
        GameObject enemyGO = new GameObject("TestEnemy");
        enemyGO.AddComponent<SpriteRenderer>();
        DisplayEnemy de = enemyGO.AddComponent<DisplayEnemy>();
        de.ActualizarID(8);

        // Añadir HealthBar hijo con BarraVidaEnemy y componentes UI mínimos
        GameObject healthBar = new GameObject("HealthBar");
        healthBar.transform.SetParent(enemyGO.transform);
        BarraVidaEnemy barra = healthBar.AddComponent<BarraVidaEnemy>();
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

        // Añadir EnemyController y asignar su Tile posición en x=7,y=2 (sin usar GridManager)
        EnemyController ec = enemyGO.AddComponent<EnemyController>();
        GameObject enemyTileGO = new GameObject("EnemyTile");
        Tile enemyTile = enemyTileGO.AddComponent<Tile>();
        enemyTile.x = 7;
        enemyTile.y = 2;
        // asignamos directamente la posicion interna del EnemyController
        ec.posicion = enemyTile;

        // Evitar Null/KeyNotFound en EnemyController.Awake/Update: asignar vidaActualEnemy > 0 y registrar en GameManager.enemigos
        var vidaField = typeof(EnemyController).GetField("vidaActualEnemy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (vidaField != null) vidaField.SetValue(ec, 1);
        if (GameManager.enemigos == null) GameManager.enemigos = new System.Collections.Generic.Dictionary<GameObject, Vector2>();
        GameManager.enemigos[enemyGO] = new Vector2(enemyTile.x, enemyTile.y);
        if (GameManager.enemigosLis == null) GameManager.enemigosLis = new System.Collections.Generic.List<GameObject>();
        GameManager.enemigosLis.Add(enemyGO);

        // Añadir TileManagerEnemigo y llamar Misilero
        TileManagerEnemigo tme = enemyGO.AddComponent<TileManagerEnemigo>();
        Vector2[] resultados = tme.Misilero(1, enemyGO);

        Vector2 enemyPos = new(ec.GetPos().x, pc.GetPos().y);
        Vector2[] lista = {new(6,2),new(5,2),new(4,2),new(3,2),new(3,3),new(3,1),new(2,2),new(2,4),new(2,0),
        new(1,2),new(1,5),new(1,-1),new(0,2),new(0,6),new(0,-2)};
        CollectionAssert.AreEquivalent(resultados,lista);
        // Limpieza
        Object.DestroyImmediate(enemyGO);
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(playerTileGO);
        Object.DestroyImmediate(enemyTileGO);
    }
}
