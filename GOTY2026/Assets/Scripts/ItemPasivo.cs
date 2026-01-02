using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPasivo", menuName = "ScriptableObjects/ItemPasivo")]
public class ItemPasivo : ScriptableObject
{
    public int id;
    public String _name;
    public String descripcion;
    public Sprite sprite;
}

