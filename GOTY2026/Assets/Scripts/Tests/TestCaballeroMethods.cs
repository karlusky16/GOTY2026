#if UNITY_EDITOR
using NUnit.Framework;
using UnityEngine;

public class TestCaballeroMethods
{
    [Test]
    public void Caballero1_GeneratesExpectedPattern()
    {
        // enemy at (7,2)
        Vector2 enemyPos = new Vector2(7, 2);
        Vector2[] expected =
        {
            new Vector2(6,2),new Vector2(6,1),new Vector2(6,3),
            new Vector2(5,2),new Vector2(5,1),new Vector2(5,3),
            new Vector2(4,2),new Vector2(4,1),new Vector2(4,3),
            new Vector2(3,2),new Vector2(3,1),new Vector2(3,3),
            new Vector2(2,2),new Vector2(2,1),new Vector2(2,3),
            new Vector2(1,2),new Vector2(1,1),new Vector2(1,3),
            new Vector2(0,2),new Vector2(0,1),new Vector2(0,3)
        };

        Vector2[] result = TileManagerEnemigo.Caballero1(0, enemyPos);

        CollectionAssert.AreEquivalent(expected, result);
    }

    [Test]
    public void Circulo_GeneratesExpectedPattern()
    {
        // create a lightweight TileManagerEnemigo instance
        var go = new GameObject("tme");
        var tme = go.AddComponent<TileManagerEnemigo>();

        Vector2 enemyPos = new Vector2(0, 2);
        Vector2[] expected = {
            new Vector2(-1, 2),new Vector2(-1, 1),new Vector2(-1, 3),
            new Vector2(1, 2),new Vector2(1, 1),new Vector2(1, 3),
            new Vector2(0, 3),new Vector2(0, 1)
        };

        Vector2[] result = tme.Circulo(0, enemyPos);

        CollectionAssert.AreEquivalent(expected, result);

        Object.DestroyImmediate(go);
    }
}
#endif
