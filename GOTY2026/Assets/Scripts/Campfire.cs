using UnityEngine;

public class Campfire : MonoBehaviour
{
    GameObject panelCartas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelCartas = GameObject.Find("DisplayCartas");
        panelCartas.SetActive(false);
    }
    public GameObject prefabCarta;
    public void Heal()
    {
        GameManager.player.GetComponent<PlayerController>().SendMessage("AumentarVida", 100);
    }

    public void DestroyCard()
    {
        panelCartas.SetActive(true);
        for (int i = 0; i < GameManager.cardList.Count; i++)
        {
            GameObject carta = Instantiate(prefabCarta, panelCartas.transform);
            DisplayCard dc = carta.GetComponent<DisplayCard>();
            dc.ActualizarID(GameManager.cardList[i].id);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
