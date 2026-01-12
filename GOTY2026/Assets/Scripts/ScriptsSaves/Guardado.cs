using System.IO;
using UnityEngine;

public class Guardado : MonoBehaviour
{
    private string rutaSave => Application.persistentDataPath + "/save.json";
    public void Guardar()
    {
        GameManager.player.GetComponent<PlayerController>().GuardarStats();
        string json = JsonUtility.ToJson(GameManager.player.GetComponent<PlayerController>().stats, true);
        File.WriteAllText(rutaSave, json);
        Debug.Log("Jugador guardado en: " + rutaSave);
    }

    public bool HayDatos()
    {
        return File.Exists(rutaSave);
    }

    public void Cargar()
    {
        if (!File.Exists(rutaSave))
        {
            Debug.Log("No hay datos guardados");
            return;
        }
        string json = File.ReadAllText(rutaSave);
        PlayerStats statsGuardadas = JsonUtility.FromJson<PlayerStats>(json);
        GameManager.player.GetComponent<PlayerController>().stats = statsGuardadas;
        Debug.Log("Jugador cargado correctamente");
    }
}