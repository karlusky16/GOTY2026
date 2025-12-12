using UnityEngine;

public class HomeStoreScene : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject shopPanel;
    public GameObject inventoryPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AbrirInicio();
    }

    /*public void AbrirTienda()
    {
        shopPanel.SetActive(true);
        homePanel.SetActive(false);
        inventoryPanel.SetActive(false);
        shopPanel.GetComponent<ShopManager>().ActualizarMonedasUI();
        shopPanel.GetComponent<ShopManager>().CardsPanelActivate();
    }*/
    /*public void AbrirInventario()
    {
        inventoryPanel.SetActive(true);
        homePanel.SetActive(false);
        shopPanel.SetActive(false);
        inventoryPanel.GetComponent<InventoryManager>().ActualizarMonedasUI();
        inventoryPanel.GetComponent<InventoryManager>().CardsPanelActivate();
    }*/
    public void AbrirInicio()
    {
        homePanel.SetActive(true);
        inventoryPanel.GetComponent<InventoryManager>().CerrarInventario();
        shopPanel.GetComponent<ShopManager>().CerrarTienda();
    }
    public void CerrarInicio()
    {
        homePanel.SetActive(false);
    }
    public void SigEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
}
