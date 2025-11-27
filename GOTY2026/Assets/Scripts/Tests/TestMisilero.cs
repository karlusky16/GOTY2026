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
        // Probaremos un enemigo en (4,2) para tener un patrón predecible
        var go = new GameObject("tme");
        var tme = go.AddComponent<TileManagerEnemigo>();

        Vector2 enemyPos = new Vector2(4, 2);
        Vector2[] resultados = tme.Misilero(0, enemyPos);

        // Esperado: inicialmente (-1,-2,-3) en X respecto al enemy -> (3,2),(2,2),(1,2)
        // luego el bucle añade i from enemyX-4 == 0 down to 0 -> solo i=0 con c=1 -> (0,2),(0,3),(0,1)
        Vector2[] esperadas = {
            new Vector2(3,2), new Vector2(2,2), new Vector2(1,2),
            new Vector2(0,2), new Vector2(0,3), new Vector2(0,1)
        };

        Assert.AreEqual(6, resultados.Length);
        CollectionAssert.AreEquivalent(esperadas, resultados);

        Object.DestroyImmediate(go);
    }
}
