using UnityEngine;

public class FondoManager : MonoBehaviour
{
    public GameObject gridManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Aparecer() {gridManager.SetActive(true); Debug.Log("gridManager activo: " + gridManager.activeSelf);}

    public void Desaparecer() {gridManager.SetActive(false); Debug.Log("gridManager activo: " + gridManager.activeSelf);}

}
