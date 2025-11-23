using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI Shop;
    public GameObject Items;
    public GameObject Cards;
    public Button ItemsBtn;
    public Button CardsBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemsActivate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardsActivate()
    {
        ItemsBtn.interactable = true;
        CardsBtn.interactable = false;
        Cards.SetActive(true);
        Items.SetActive(false);
    }
    public void ItemsActivate()
    {
        ItemsBtn.interactable = false;
        CardsBtn.interactable = true;
        Items.SetActive(true);
        Cards.SetActive(false);
    }
    public void SigEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapUi");
    }
}
