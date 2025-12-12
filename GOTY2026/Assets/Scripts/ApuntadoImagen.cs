using UnityEngine;
using UnityEngine.EventSystems;

public class ApuntadoImagen : MonoBehaviour
{
 public void OnPointerEnter(PointerEventData eventData)
    {
        TurnManager.noMas.text = "Estas apuntado por un francotirador, sino lo matas te hará daño al final del turno";
        Invoke("OcultarMensaje",1f);
    }
    void OcultarMensaje()
    {
        TurnManager.noMas.text = "";
    }
}
