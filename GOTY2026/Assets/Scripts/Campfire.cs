using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Campfire : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;
    GameObject panelCartas;
    GameObject textoAlerta;
    GameObject noDestruir;
    bool matado;
    bool curado;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        matado = false;
        noDestruir = GameObject.Find("NoDestroy");
        textoAlerta = GameObject.Find("TextoAlerta");
        panelCartas = GameObject.Find("DisplayCartas");
        noDestruir.SetActive(false);
        panelCartas.SetActive(false);
        textoAlerta.GetComponent<TextMeshProUGUI>().text = "";
    }
    public GameObject prefabCarta;
    public void Heal()
    {
        if (curado)
        {
            textoAlerta.GetComponent<TextMeshProUGUI>().text = "El jugador ya se ha curado";
            Invoke(nameof(OcultarMensaje), 1f);
            return;
        }
        else if (matado) {
            textoAlerta.GetComponent<TextMeshProUGUI>().text = "El jugador ha eliminado una carta y por tanto no puede curar";
        }
        else if (GameManager.player.GetComponent<PlayerController>().GetVidaActual() == GameManager.player.GetComponent<PlayerController>().GetVidaMaxima())
        {
            textoAlerta.GetComponent<TextMeshProUGUI>().text = "El jugador tiene la vida al máximo";
            Invoke(nameof(OcultarMensaje), 1f);
            return;
        }
        GameManager.player.GetComponent<PlayerController>().SendMessage("AumentarVida", GameManager.player.GetComponent<PlayerController>().GetVidaMaxima()/2);
        curado = true;
    }

    public void DestroyCard()
    {   
        audioSource.PlayOneShot(pulsarBotonClip);
        if (matado)
        {
            textoAlerta.GetComponent<TextMeshProUGUI>().text = "El jugador ya ha eliminado una carta";
            Invoke(nameof(OcultarMensaje), 1f);
            return;
        }
        else if (curado)
        {
            textoAlerta.GetComponent<TextMeshProUGUI>().text = "El jugador ya se ha curado y por tanto no puedes eliminar carta";
            Invoke(nameof(OcultarMensaje), 1f);
            return;
        }
        panelCartas.SetActive(true);
        noDestruir.SetActive(true);
        for (int i = 0; i < GameManager.player.GetComponent<PlayerController>().GetCartasLength(); i++)
        {
            var carta = Instantiate(prefabCarta,panelCartas.transform);
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
        audioSource.PlayOneShot(pulsarBotonClip);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    public void NoDestruir()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
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
        textoAlerta.GetComponent<TextMeshProUGUI>().text = "";
    }
    void Update()
    {
        
    }
}
