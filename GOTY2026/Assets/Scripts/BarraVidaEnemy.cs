using UnityEngine;
using UnityEngine.UI;

public class BarraVidaEnemy : MonoBehaviour
{
    [SerializeField] private Slider sliderVidaEnemy;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private Text textoVida;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyController = FindFirstObjectByType<EnemyController>();

        enemyController.EnemyReduceVida += ReducirBarraDeVidaEnemy;

        IniciarBarraDeVidaEnemy(enemyController.GetVidaMaxima(), enemyController.GetVidaActual());
    }

    void OnDisable()
    {
        enemyController.EnemyReduceVida -= ReducirBarraDeVidaEnemy;
    }
    
    private void IniciarBarraDeVidaEnemy(int vidaMaximaEnemy, int vidaActualEnemy)
    {
        sliderVidaEnemy.maxValue = vidaMaximaEnemy;
        sliderVidaEnemy.value = vidaActualEnemy;
        textoVida.text = vidaActualEnemy.ToString();
    }

    public void ReducirBarraDeVidaEnemy(int vidaActualEnemy)
    {
        sliderVidaEnemy.value = vidaActualEnemy;
        textoVida.text = vidaActualEnemy.ToString();
        Debug.Log("Reduce barra vida enemy");

    }
    /*public void AumentarBarraDeVidaPlayer(int vidaActual)
    {
        sliderVidaPlayer.value = vidaActual;
        textoVida.text = vidaActual.ToString();
        Debug.Log("Aumenta barra vida");
    }*/
}
