using System;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObjects/Event")]
public class Events : ScriptableObject
{
    public int id;
    public String nombre;
    public String texto;
    public String textBoton1;
    public String textBoton2;
    public int costeMonedas1;
    public int costeMonedas2;
    public int costeMana1;
    public int costeMana2;
    public int costeEnergia1;
    public int costeEnergia2;
    public int costeVida1;
    public int costeVida2;
    public Texture sprite;
}
