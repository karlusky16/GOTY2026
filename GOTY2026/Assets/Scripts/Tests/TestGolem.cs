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
        var go = new GameObject("tme");
        var tme = go.AddComponent<TileManagerEnemigo>();
        Vector2[] resultados = tme.Golem(new Vector2(3,2));

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
    }
}