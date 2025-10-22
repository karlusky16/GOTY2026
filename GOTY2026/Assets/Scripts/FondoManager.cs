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
        if (TurnManager.cartaSeleccionada)
        {
            gridManager.SetActive(true);
        }
        else
        {
            gridManager.SetActive(false);
        }

    }
}
