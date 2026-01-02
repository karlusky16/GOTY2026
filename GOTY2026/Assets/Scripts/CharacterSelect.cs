using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pulsarBotonClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SigEscena()
    {
        audioSource.PlayOneShot(pulsarBotonClip);
        Debug.Log("Cargando escena de muestra...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
