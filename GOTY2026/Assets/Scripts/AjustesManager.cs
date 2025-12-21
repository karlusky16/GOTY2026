using System.IO;
using TMPro;
using UnityEngine;

public class AjustesManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;
    public Ajustes ajustes;
    public GameObject panelAjustes;
    public GameObject panelAjustes2;
    private string RutaSave => Application.persistentDataPath + "/save.json";
    private string RutaSaveAj => Application.persistentDataPath + "/ajustes.json";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!File.Exists(RutaSaveAj))
        {
            ajustes ??= new Ajustes();
            string json = JsonUtility.ToJson(ajustes, true);
        }
        else
        {
            string json = File.ReadAllText(RutaSaveAj);
            ajustes = JsonUtility.FromJson<Ajustes>(json);
        }
        GameObject.Find("Estamina").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Estamina = " + ajustes.estamina;
        panelAjustes = GameObject.Find("OpcPanel");
        panelAjustes2 = GameObject.Find("PanelOpc");
        panelAjustes.SetActive(false);
        panelAjustes2.SetActive(false);
    }
    public void ActivarPrimerMenu()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        panelAjustes.SetActive(!panelAjustes.activeSelf);
    }
    public void Salir()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        Application.Quit();
    }
    public void Guardar()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        GameObject.Find("Player").GetComponent<PlayerController>().GuardarStats();
        string json = JsonUtility.ToJson(GameManager.player.GetComponent<PlayerController>().stats, true);
        File.WriteAllText(RutaSave, json);
    }
    public void ActivarSegundoMenu()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        panelAjustes2.SetActive(!panelAjustes2.activeSelf);
    }
    public void Stamina()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        if (!GameManager.estamina)
        {
            GameObject.Find("Estamina").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Estamina = true";
            GameManager.estamina = true;
        }
        else
        {
            GameObject.Find("Estamina").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Estamina = false";
            GameManager.estamina = false; 
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
