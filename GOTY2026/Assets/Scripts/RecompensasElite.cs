using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RecompensasElite : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;
    [SerializeField] GameObject panelCartas;
    void Start()
    {
        // If not assigned in inspector, try to find the panel in-scene (including inactive objects)
        if (panelCartas == null)
        {
            panelCartas = FindInSceneByName("DisplayCartas") ?? FindInSceneByName("PanelCartas");
        }

        if (panelCartas == null)
        {
            Debug.LogWarning("RecompensasScript: panelCartas not found. Assign it in the inspector or name it 'DisplayCartas' or 'PanelCartas'.");
        }
        else
        {
            panelCartas.SetActive(false);
        }
    }

    // Recursively search scene root objects for a child/gameobject with the given name.
    GameObject FindInSceneByName(string name)
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var roots = scene.GetRootGameObjects();
        foreach (var root in roots)
        {
            var found = FindRecursive(root.transform, name);
            if (found != null) return found;
        }
        return null;
    }

    GameObject FindRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent.gameObject;
        for (int i = 0; i < parent.childCount; i++)
        {
            var c = parent.GetChild(i);
            var r = FindRecursive(c, name);
            if (r != null) return r;
        }
        return null;
    }
    public GameObject prefabCarta;   // tu prefab de carta
    public void SigEscena()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    public void RecogerDinero()
    {
        Debug.Log("Dinero recogido");
        GameManager.player.GetComponent<PlayerController>().AumentarMonedas(50);
        GameObject.Find("BotonDinero").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
    public void RecogerCarta()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
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
        audioSource.PlayOneShot(pulsarBotonClip);
        Debug.Log(carta.GetComponent<DisplayCard>().displayID);
        GameManager.player.GetComponent<PlayerController>().AddCarta(carta.GetComponent<DisplayCard>().displayID);
        panelCartas.SetActive(false);
    }
    public void CerrarPanelSinCarta()
    {
        panelCartas.SetActive(false);
    }

}
