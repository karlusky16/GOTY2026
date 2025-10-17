using System;
using Unity.VisualScripting;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject prefabCarta;   // tu prefab de carta
    public Transform parentCanvas;   // referencia al Canvas en la escena
    public GameObject prefabEnemigo;

    void Start()
    {
        // Si la instancia no existe, crea una y marca el objeto para no ser destruido.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Player.cartas.Add(1);
        Player.cartas.Add(1);
        Player.cartas.Add(1);
        Player.cartas.Add(1);
        Player.cartas.Add(1);
        mano();
        GridManager._tiles[new Vector2(1,1)].ocupadoObj = Instantiate(prefabEnemigo, GridManager._tiles[new Vector2(1,1)].transform.position,Quaternion.identity);
        GridManager._tiles[new Vector2(1, 1)].ocupado = true;
    }
    void mano()
    {
        System.Random rand = new();
        int cartas = Player.cartas.Count;
        GameObject[] mano = new GameObject[Player.longMano];
        float xMin = -250f;
        float xMax = 250f;
        for (int i = 0; i < Player.longMano; i++)
        {
            int indiceAleatorio = rand.Next(cartas);
            mano[i] = Instantiate(prefabCarta, parentCanvas);
            mano[i].GetComponent<DisplayCard>().displayID = Player.cartas[indiceAleatorio];
            float xPos = xMin + i * (xMax - xMin) / (cartas - 1);
            RectTransform rt = mano[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(xPos, -100);
        }
  
    }
    
}
