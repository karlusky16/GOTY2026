using UnityEngine;
using UnityEngine.UI;

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
        for (int i = 0; i < GameManager.player.GetComponent<PlayerController>().GetCartasLength(); i++)
        {
            var carta = Instantiate(prefabCarta, panelCartas.transform);
            DisplayCard dc = carta.GetComponent<DisplayCard>();
            int idLocal = GameManager.player.GetComponent<PlayerController>().GetCartas()[i];
            dc.ActualizarID(idLocal);

            Button btn = carta.GetComponentInChildren<Button>();
            btn.onClick.RemoveAllListeners();
            Debug.Log("Asignando listener a la carta con ID: " + i);
            btn.onClick.AddListener(() => MatarCarta(idLocal));
        }
    }

    public void MatarCarta(int id)
    {
        Debug.Log("Carta eliminada: " + id);
        GameManager.player.GetComponent<PlayerController>().GetCartas().Remove(id);
        panelCartas.SetActive(false);

    }
    public void SigEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
