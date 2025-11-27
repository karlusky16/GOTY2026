using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class TestPatronesCarta
{
    // helper to set GameManager.player
    GameObject CreatePlayerAt(int x, int y)
    {
        var playerGO = new GameObject("Player");
        var pc = playerGO.AddComponent<PlayerController>();
        var tileGO = new GameObject("PlayerTile");
        var tile = tileGO.AddComponent<Tile>();
        tile.x = x;
        tile.y = y;
        pc.posicion = tile;
        GameManager.player = playerGO;
        return playerGO;
    }

    GameObject CreateCard(int tipo = 0, int area = 1, int area2 = 1)
    {
        var cardGO = new GameObject("Card");
        var dc = cardGO.AddComponent<DisplayCard>();
        var c = ScriptableObject.CreateInstance<Card>();
        c.tipo = tipo;
        c.area = area;
        c.area2 = area2;
        dc.card = c;
        GameManager.carta = cardGO;
        return cardGO;
    }

    [Test]
    public void Cruz_ReturnsExpectedOffsets()
    {
        // invoke private static Cruz(int area)
        Type t = typeof(TileManager);
        MethodInfo cruz = t.GetMethod("Cruz", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(cruz, "Cruz method not found");

        var result = (Vector2[])cruz.Invoke(null, new object[] { 1 });
        // area=1 -> length = area*4 +1 = 5
        Assert.AreEqual(5, result.Length);
        // should contain origin and the four adjacent directions
        CollectionAssert.AreEquivalent(new Vector2[] { new Vector2(0,0), new Vector2(-1,0), new Vector2(1,0), new Vector2(0,-1), new Vector2(0,1) }, result);
    }

    [Test]
    public void Recta_ReturnsDirectionalOffsets()
    {
        // player at (2,2), target tile at (4,2) -> direction = (2,0)
        var player = CreatePlayerAt(2,2);
        var card = CreateCard(tipo:0, area:3);

        // create a Tile for argument
        var targetGO = new GameObject("tile");
        var tile = targetGO.AddComponent<Tile>();
        tile.x = 4; tile.y = 2;

        Type t = typeof(TileManager);
        MethodInfo recta = t.GetMethod("Recta", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(recta);

        var result = (Vector2[])recta.Invoke(null, new object[] { 3, tile });
        // expected: origin, direccion*1, direccion*2 where direccion = (tile - player) = (2,0)
        var expected = new Vector2[] { new Vector2(0,0), new Vector2(2,0), new Vector2(4,0) };
        CollectionAssert.AreEqual(expected, result);

        UnityEngine.Object.DestroyImmediate(player);
        UnityEngine.Object.DestroyImmediate(GameManager.carta);
        UnityEngine.Object.DestroyImmediate(targetGO);
    }

    [Test]
    public void RectaH_ReturnsVerticalBand()
    {
        var player = CreatePlayerAt(3,2);
        var card = CreateCard(tipo:0, area:4);
        var tileGO = new GameObject("tile");
        var tile = tileGO.AddComponent<Tile>();
        tile.x = 3; tile.y = 2;

        Type t = typeof(TileManager);
        MethodInfo rectaH = t.GetMethod("RectaH", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(rectaH);

        var res = (Vector2[])rectaH.Invoke(null, new object[] { 4, tile });
        // origin + (0,1),(0,-1)
        var expected = new Vector2[] { new Vector2(0,0), new Vector2(0,1), new Vector2(0,-1) };
        CollectionAssert.AreEqual(expected, res);

        UnityEngine.Object.DestroyImmediate(player);
        UnityEngine.Object.DestroyImmediate(GameManager.carta);
        UnityEngine.Object.DestroyImmediate(tileGO);
    }

    [Test]
    public void Rectangulo_GeneratesCorrectGrid()
    {
        // player at (0,0), tile at (2,0) -> direction Right
        var player = CreatePlayerAt(0,0);
        var card = CreateCard(tipo:0);
        var tileGO = new GameObject("tile");
        var tile = tileGO.AddComponent<Tile>();
        // use a unit direction (1,0) relative to player at (0,0)
        tile.x = 1; tile.y = 0;

        Type t = typeof(TileManager);
        MethodInfo rectangulo = t.GetMethod("Rectangulo", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(rectangulo);

        // area=2, area2=3 => should produce 6 positions
        var res = (Vector2[])rectangulo.Invoke(null, new object[] { 2, 3, tile });
        Assert.AreEqual(6, res.Length);
        // since direction is right, positions should be (i,j) with i from 0..1 and j from -1..1
        // expected order per implementation: for i=0..area2-1, j=-areaD2..areaD2 => (0,-1),(0,0),(0,1),(1,-1),(1,0),(1,1)
        var expected = new Vector2[] { new Vector2(0,-1), new Vector2(0,0), new Vector2(0,1), new Vector2(1,-1), new Vector2(1,0), new Vector2(1,1) };
        CollectionAssert.AreEquivalent(expected, res);

        UnityEngine.Object.DestroyImmediate(player);
        UnityEngine.Object.DestroyImmediate(GameManager.carta);
        UnityEngine.Object.DestroyImmediate(tileGO);
    }

    [Test]
    public void TresDir_ReturnsThreeDirections()
    {
        // player at (2,2), tile at (4,2) -> horizontal direction
        var player = CreatePlayerAt(2,2);
        var card = CreateCard(tipo:0);
        var tileGO = new GameObject("tile");
        var tile = tileGO.AddComponent<Tile>();
        tile.x = 4; tile.y = 2;

        Type t = typeof(TileManager);
        MethodInfo tresdir = t.GetMethod("TresDir", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(tresdir);

        var res = (Vector2[][])tresdir.Invoke(null, new object[] { 2, tile });
        // Verify the exact directional vectors produced by TresDir for horizontal direction
        // Expect:
        // direcciones[0] = (0,0), (2,0), (4,0)
        // direcciones[1] = (2,-1), (4,-2)
        // direcciones[2] = (2,1), (4,2)
        Vector2[] exp0 = { new Vector2(0,0), new Vector2(2,0), new Vector2(4,0) };
        Vector2[] exp1 = { new Vector2(2,-1), new Vector2(4,-2) };
        Vector2[] exp2 = { new Vector2(2,1), new Vector2(4,2) };

        CollectionAssert.AreEqual(exp0, res[0]);
        CollectionAssert.AreEqual(exp1, res[1]);
        CollectionAssert.AreEqual(exp2, res[2]);

        UnityEngine.Object.DestroyImmediate(player);
        UnityEngine.Object.DestroyImmediate(GameManager.carta);
        UnityEngine.Object.DestroyImmediate(tileGO);
    }

    [Test]
    public void ObtenerTilesEnRango_FindsTilesWithinManhattan()
    {
        // Setup a small grid 5x5 in GridManager._tiles
        GridManager._tiles = new System.Collections.Generic.Dictionary<Vector2, Tile>();
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var go = new GameObject($"Tile {x} {y}");
                var tile = go.AddComponent<Tile>();
                tile.x = x; tile.y = y;
                GridManager._tiles[new Vector2(x, y)] = tile;
            }
        }

        // create an instance of TileManager
        var goTM = new GameObject("tm");
        var tm = goTM.AddComponent<TileManager>();

        // center tile at (2,2)
        var center = GridManager._tiles[new Vector2(2,2)];
        // call private instance method ObtenerTilesEnRango
        MethodInfo obtener = typeof(TileManager).GetMethod("ObtenerTilesEnRango", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(obtener);

        var list = (System.Collections.Generic.List<Tile>)obtener.Invoke(tm, new object[] { center, 1 });
        // with manhattan range 1, expect 4 neighbors at positions (1,2),(3,2),(2,1),(2,3)
        Vector2[] expectedNeighbors = { new Vector2(1,2), new Vector2(3,2), new Vector2(2,1), new Vector2(2,3) };
        var actual = list.Select(t => new Vector2(t.x, t.y)).ToArray();
        CollectionAssert.AreEquivalent(expectedNeighbors, actual);

        // cleanup: destroy created GameObjects
        foreach (var kv in GridManager._tiles)
        {
            UnityEngine.Object.DestroyImmediate(kv.Value.gameObject);
        }
        GridManager._tiles = null;
        UnityEngine.Object.DestroyImmediate(goTM);
    }

    [Test]
    public void Rectangulo2_GeneratesCenteredGrid()
    {
        // create a dummy tile (Rectangulo2 returns offsets, independent of tile position)
        var tileGO = new GameObject("tileRect2");
        var tile = tileGO.AddComponent<Tile>();
        tile.x = 5; tile.y = 5;

        Type t = typeof(TileManager);
        MethodInfo rect2 = t.GetMethod("Rectangulo2", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(rect2, "Rectangulo2 method not found");

        // area=3, area2=3 should produce 9 offsets centered around (0,0)
        var res = (Vector2[])rect2.Invoke(null, new object[] { 3, 3, tile });

        Vector2[] expected = {
            new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1),
            new Vector2(0, -1), new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(1, -1), new Vector2(1, 0), new Vector2(1, 1)
        };

        // verify exact order and values
        CollectionAssert.AreEqual(expected, res);

        UnityEngine.Object.DestroyImmediate(tileGO);
    }
}
