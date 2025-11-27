using UnityEngine;
using UnityEngine.UI;

public class Campfire : MonoBehaviour
{
    GameObject panelCartas;
    GameObject yaCurado;
    GameObject yaRoto;
    GameObject noDestruir;
    bool matado;
    bool curado;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        matado = false;
        noDestruir = GameObject.Find("NoDestroy");
        yaCurado = GameObject.Find("YaCurado");
        yaRoto = GameObject.Find("YaDestroy");
        panelCartas = GameObject.Find("DisplayCartas");
        noDestruir.SetActive(false);
        panelCartas.SetActive(false);
        yaRoto.SetActive(false);
        yaCurado.SetActive(false); 
    }
    public GameObject prefabCarta;
    public void Heal()
    {
        if (curado || (GameManager.player.GetComponent<PlayerController>().GetVidaActual() == GameManager.player.GetComponent<PlayerController>().GetVidaMaxima()) || matado)
        {
            Debug.Log("La vida ya está al máximo");
            yaRoto.SetActive(false);
            yaCurado.SetActive(true);
            Invoke(nameof(OcultarMensaje), 1f);
            return;
        }
        GameManager.player.GetComponent<PlayerController>().SendMessage("AumentarVida", GameManager.player.GetComponent<PlayerController>().GetVidaMaxima()/2);
        curado = true;
    }

    public void DestroyCard()
    {   if (matado || curado)
        {
            Debug.Log("Ya se ha eliminado una carta en esta fogata.");
            yaCurado.SetActive(false);
            yaRoto.SetActive(true);
            Invoke(nameof(OcultarMensaje), 1f);
            return;
        }
        panelCartas.SetActive(true);
        noDestruir.SetActive(true);
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
        GameManager.player.GetComponent<PlayerController>().RemoveCarta(id);
        Debug.Log("Cartas restantes: " + GameManager.player.GetComponent<PlayerController>().GetCartasLength());
        for (int i = panelCartas.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(panelCartas.transform.GetChild(i).gameObject);
        }
        panelCartas.SetActive(false);
        noDestruir.SetActive(false);
        matado = true;
    }
    public void SigEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    public void NoDestruir()
    {
        for (int i = panelCartas.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(panelCartas.transform.GetChild(i).gameObject);
        }
        panelCartas.SetActive(false);
        noDestruir.SetActive(false);
    }
    // Update is called once per frame
    void OcultarMensaje()
    {
        yaCurado.SetActive(false);
        yaRoto.SetActive(false);
    }
    void Update()
    {
        
    }
}
