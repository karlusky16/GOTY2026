using UnityEngine;
using UnityEngine.UI;

public class BarraVidaPlayer : MonoBehaviour
{
    [SerializeField] private Slider sliderVidaPlayer;
    [SerializeField] private PlayerController vidaPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaPlayer = FindFirstObjectByType<PlayerController>();

        vidaPlayer.JugadorRecibeDaño += CambiarBarraDeVidaPlayer;

        IniciarBarraDeVidaPlayer(vidaPlayer.GetVidaMaxima(), vidaPlayer.GetVidaActual());
    }

    void OnDisable()
    {
        vidaPlayer.JugadorRecibeDaño -= CambiarBarraDeVidaPlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void IniciarBarraDeVidaPlayer(int vidaMaxima, int vidaActual)
    {
        sliderVidaPlayer.maxValue = vidaMaxima;
        sliderVidaPlayer.value = vidaActual;
    }

    public void CambiarBarraDeVidaPlayer(int vidaActual)
    {
        sliderVidaPlayer.value = vidaActual;
    }
}
