using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new();
    void Awake()
    {
        cardList.Add(new Card(1,"FireBall",1,0,"Disparas una bola de fuego que forma una cruz desde la casilla seleccionada"));
    }
}
