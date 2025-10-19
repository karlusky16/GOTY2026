using UnityEngine;
using UnityEngine.UI;

public class BarraManaPlayer : MonoBehaviour
{
    [SerializeField] private Slider sliderManaPlayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Text textoMana;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        playerController.JugadorReduceMana += ReducirBarraDeManaPlayer;
        playerController.JugadorAumentaMana += AumentarBarraDeManaPlayer;

        IniciarBarraDeManaPlayer(playerController.GetManaMaxima(), playerController.GetManaActual());
    }

    void OnDisable()
    {
        playerController.JugadorReduceMana -= ReducirBarraDeManaPlayer;
        playerController.JugadorAumentaMana -= AumentarBarraDeManaPlayer;
    }
    
    private void IniciarBarraDeManaPlayer(int manaMaxima, int manaActual)
    {
        sliderManaPlayer.maxValue = manaMaxima;
        sliderManaPlayer.value = manaActual;
        textoMana.text = manaActual.ToString();
    }

    public void ReducirBarraDeManaPlayer(int manaActual)
    {
        sliderManaPlayer.value = manaActual;
        textoMana.text = manaActual.ToString();
        Debug.Log("Reduce mana Barra");
    }
    public void AumentarBarraDeManaPlayer(int manaActual)
    {
        sliderManaPlayer.value = manaActual;
        textoMana.text = manaActual.ToString();
    }
}
