using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        // Si la instancia no existe, crea una y marca el objeto para no ser destruido.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
