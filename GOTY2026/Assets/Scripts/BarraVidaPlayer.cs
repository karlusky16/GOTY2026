using UnityEngine;
using UnityEngine.UI;

public class BarraVidaPlayer : MonoBehaviour
{
    [SerializeField] private Slider sliderVidaPlayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Text textoVida;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        playerController.JugadorReduceVida += ReducirBarraDeVidaPlayer;
        playerController.JugadorAumentaVida += AumentarBarraDeVidaPlayer;

        IniciarBarraDeVidaPlayer(playerController.GetVidaMaxima(), playerController.GetVidaActual());
    }

    void OnDisable()
    {
        playerController.JugadorReduceVida -= ReducirBarraDeVidaPlayer;
        playerController.JugadorAumentaVida -= AumentarBarraDeVidaPlayer;
    }
    
    private void IniciarBarraDeVidaPlayer(int vidaMaxima, int vidaActual)
    {
        sliderVidaPlayer.maxValue = vidaMaxima;
        sliderVidaPlayer.value = vidaActual;
        textoVida.text = vidaActual.ToString();
    }

    public void ReducirBarraDeVidaPlayer(int vidaActual)
    {
        sliderVidaPlayer.value = vidaActual;
        textoVida.text = vidaActual.ToString();

    }
    public void AumentarBarraDeVidaPlayer(int vidaActual)
    {
        sliderVidaPlayer.value = vidaActual;
        textoVida.text = vidaActual.ToString();

    }
}
