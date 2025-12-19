using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public PlayerController player;
    private string rutaSave => Application.persistentDataPath + "/save.json";
    public void Guardar()
    {
        string json = JsonUtility.ToJson(player.stats, true);
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
        player.stats = statsGuardadas;
        Debug.Log("Jugador cargado correctamente");
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}