using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RecompensasScript : MonoBehaviour
{
    GameObject panelCartas;
    void Start()
    {
        panelCartas = GameObject.Find("DisplayCartas");
        panelCartas.SetActive(false);
    }
    public GameObject prefabCarta;   // tu prefab de carta
    public void SigEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    public void RecogerDinero()
    {
        Debug.Log("Dinero recogido");
        GameObject.Find("BotonDinero").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
    public void RecogerCarta()
    {
        Debug.Log("Carta recogida");
        MostrarCartasRecompensa();
        GameObject.Find("BotonVerCartas").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
    public void MostrarCartasRecompensa()
    {
        panelCartas.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, GameManager.cardList.Count);
            panelCartas.transform.GetChild(i).GetComponent<DisplayCard>().ActualizarID(randomIndex + 1);
        }
    }
    public void CerrarPanelCartas(GameObject carta)
    {
        Debug.Log(carta.GetComponent<DisplayCard>().displayID);
        GameManager.player.GetComponent<PlayerController>().AddCarta(carta.GetComponent<DisplayCard>().displayID);
        panelCartas.SetActive(false);
    }

}
