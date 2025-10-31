using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new();
    void Awake()
    {
        cardList.Add(new Card(1, "FireBall", "Cruz", 2, 0, "Disparas una bola de fuego que forma una cruz desde la casilla seleccionada"
        , 4, 1, 0, 3));
        cardList.Add(new Card(2, "Rayo", "Cruz", 1, 0, "Disparas un rayo que daña a una casilla"
        , 6, 0, 0, 4));
        cardList.Add(new Card(3, "Disparo", "RectaNP", 1, 1, "Disparas una bala que no atraviesa enemigos"
        , 1, 4, 0, 3));
        cardList.Add(new Card(4, "Laser", "TresDirNP", 1, 1, "Dispara con un laser en 3 direcciones que daña hasta a un enemigo"
        , 1, 9, 0, 2));

    }
}
