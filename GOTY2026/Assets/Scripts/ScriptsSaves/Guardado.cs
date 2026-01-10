using System.IO;
using UnityEngine;

public class Guardado : MonoBehaviour
{
    private string rutaSave => Application.persistentDataPath + "/save.json";
    public void Guardar()
    {
        GameManager.player.GetComponent<PlayerController>().GuardarStats();
        SaveData saveData = new SaveData
        {
            escena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            stats = GameManager.player.GetComponent<PlayerController>().stats
        };
        string json = JsonUtility.ToJson(saveData, true);
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
        SaveData saveGuardada = JsonUtility.FromJson<SaveData>(json);
        PlayerStats statsGuardadas = saveGuardada.stats;
        GameManager.player.GetComponent<PlayerController>().stats = statsGuardadas;
        Debug.Log("Jugador cargado correctamente");
    }
}