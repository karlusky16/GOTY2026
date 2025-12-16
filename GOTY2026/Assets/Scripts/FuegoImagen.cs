using UnityEngine;
using UnityEngine.EventSystems;

public class FuegoImagen : MonoBehaviour
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TurnManager.noMas.text = "El personaje con este símbolo perdera puntos de vida correspondiente a las cargas de fuego y disminuará en uno la cantidad de cargas";
        Invoke("OcultarMensaje",1f);
    }
    void OcultarMensaje()
    {
        TurnManager.noMas.text = "";
    }

}
