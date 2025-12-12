using UnityEngine;
using UnityEngine.EventSystems;

public class AturdidoImagen : MonoBehaviour
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TurnManager.noMas.text = "El personaje con este símbolo esta aturdido, y por tatno no atacará en este turno";
        Invoke("OcultarMensaje",1f);
    }
    void OcultarMensaje()
    {
        TurnManager.noMas.text = "";
    }
}
