using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject prefabCarta;   // prefab de carta
    public Transform parentCanvas;   // referencia al Canvas en la escena
    public GameObject prefabEnemigo;
    public GameObject prefabPlayer;
    public Image _image;
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
        GridManager._tiles[new Vector2(1, 1)].ocupadoObj = Instantiate(prefabEnemigo, GridManager._tiles[new Vector2(1, 1)].transform.position, Quaternion.identity);
        GridManager._tiles[new Vector2(1, 1)].ocupado = true;
        GridManager._tiles[new Vector2(0,0)].ocupadoObj = Instantiate(prefabPlayer, GridManager._tiles[new Vector2(0,0)].transform.position,Quaternion.identity);
        GridManager._tiles[new Vector2(0, 0)].ocupado = true;
    }
    void mano()
    {
        System.Random rand = new();
        int cartas = Player.cartas.Count;
        GameObject[] mano = new GameObject[Player.longMano];


        RectTransform imageRect = _image.GetComponent<RectTransform>();
        float imageWidth = imageRect.rect.width;
        float imageHeight = imageRect.rect.height;
        float margen = 50f; // margen lateral para que no estén tan al borde
        float espacioDisponible = imageWidth - 2 * margen;
        float espacioEntreCartas = espacioDisponible / Player.longMano;
        for (int i = 0; i < Player.longMano; i++)
        {
            int indiceAleatorio = rand.Next(cartas);
            mano[i] = Instantiate(prefabCarta, _image.transform);
            mano[i].GetComponent<DisplayCard>().displayID = Player.cartas[indiceAleatorio];
        }
    }
    
}
