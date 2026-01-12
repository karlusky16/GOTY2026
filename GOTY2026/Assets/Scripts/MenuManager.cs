using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;
    private string rutaSave => Application.persistentDataPath + "/save.json";
    public Sprite sprite;
    public GameObject prefabPlayer;
    public void NuevaPartida()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        if (File.Exists(rutaSave))
        {
            File.Delete(rutaSave);
            Debug.Log("Archivo de guardado eliminado");
        }
        PlayerPrefs.DeleteKey("Map");
        PlayerPrefs.Save();
        SceneManager.LoadScene("SelectCharacter");
    }
    public void CargarPartida()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        string json = File.ReadAllText(rutaSave);
        PlayerStats statsGuardadas = JsonUtility.FromJson<PlayerStats>(json);
        GameManager.player.GetComponent<PlayerController>().CargarStats(statsGuardadas);
        SceneManager.LoadScene("MapUI");
    }
}
