using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardActionRec : MonoBehaviour
{
    public static GameObject carta;
    public GameObject player;
    void Start()
    {
        player = GameManager.player;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Al dar click se añade la carta a la baraja del jugador
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Carta recogida");
            player.GetComponent<PlayerController>().AddCarta(gameObject.GetComponent<DisplayCard>().displayID);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
