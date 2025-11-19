using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SigEscena()
    {
        Debug.Log("Cargando escena de muestra...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
