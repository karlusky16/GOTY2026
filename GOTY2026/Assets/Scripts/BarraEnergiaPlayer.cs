using UnityEngine;
using UnityEngine.UI;

public class BarraEnergiaPlayer : MonoBehaviour
{
    [SerializeField] private Slider sliderEnergiaPlayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Text textoEnergia;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        playerController.JugadorReduceEnergia += ReducirBarraDeEnergiaPlayer;
        playerController.JugadorAumentaEnergia += AumentarBarraDeEnergiaPlayer;

        IniciarBarraDeEnergiaPlayer(playerController.GetEnergiaMaxima(), playerController.GetEnergiaActual());
    }

    void OnDisable()
    {
        playerController.JugadorReduceEnergia -= ReducirBarraDeEnergiaPlayer;
        playerController.JugadorAumentaEnergia -= AumentarBarraDeEnergiaPlayer;
    }
    
    private void IniciarBarraDeEnergiaPlayer(int energiaMaxima, int energiaActual)
    {
        sliderEnergiaPlayer.maxValue = energiaMaxima;
        sliderEnergiaPlayer.value = energiaActual;
        textoEnergia.text = energiaActual.ToString();
    }

    public void ReducirBarraDeEnergiaPlayer(int energiaActual)
    {
        sliderEnergiaPlayer.value = energiaActual;
        textoEnergia.text = energiaActual.ToString();
        Debug.Log("Reduce barra energia");
    }
    public void AumentarBarraDeEnergiaPlayer(int energiaActual)
    {
        sliderEnergiaPlayer.value = energiaActual;
        textoEnergia.text = energiaActual.ToString();
        Debug.Log("Aumenta barra energia");
    }
}
