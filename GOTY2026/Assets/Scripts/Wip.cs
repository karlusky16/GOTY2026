using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wip : MonoBehaviour
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