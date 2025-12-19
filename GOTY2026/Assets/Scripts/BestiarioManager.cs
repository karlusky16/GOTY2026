
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BestiarioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject panel;
    public TextMeshProUGUI nombre;
    public Image sprite;
    public Sprite fuegoS;
    public Sprite aturdidoS;
    public Sprite mirillaS;
    private bool enP = true;
    void Start()
    {
        CambiarEstado();
    }
    public void CambiarEstado()
    {
        if (enP)
        {
            enP = false;
            foreach (Transform child in panel.transform)
            {
                Destroy(child.gameObject);
            }
            gameObject.transform.position = new(gameObject.transform.position.x + 1000, gameObject.transform.position.y);
        }
        else
        {
            enP = true;
            gameObject.transform.position = new(gameObject.transform.position.x - 1000, gameObject.transform.position.y);
        }
        
    }
    public void DisplayDatos(GameObject personaje)
    {
        if (personaje.CompareTag("Player"))
        {
            nombre.text = "Player";
            sprite.sprite = personaje.GetComponent<SpriteRenderer>().sprite;
            PlayerController pc = GameManager.player.GetComponent<PlayerController>();
            if (pc.danoFuego > 0)
            {
                GameObject fuego = new GameObject("Fuego", typeof(RectTransform));
                fuego.AddComponent<Image>().sprite = fuegoS;
                fuego.GetComponent<Image>().SetNativeSize();
                GameObject textoF = new("textoF", typeof(RectTransform));
                textoF.AddComponent<TextMeshProUGUI>().text = "El personaje tiene " + pc.danoFuego + " cargas de fuego, recibira daño equivalente a las cargas al final del turno y el contador disminuirá en uno.";
                textoF.GetComponent<TextMeshProUGUI>().fontSize = 18;
                fuego.transform.SetParent(panel.transform, false);
                textoF.transform.SetParent(panel.transform, false); 
            }
            if (pc.shock)
            {
                GameObject at = new GameObject("at", typeof(RectTransform));
                at.AddComponent<Image>().sprite = aturdidoS;
                at.GetComponent<Image>().SetNativeSize();
                GameObject textoAt = new("textoAt", typeof(RectTransform));
                textoAt.AddComponent<TextMeshProUGUI>().text = "El personaje esta aturdido, y por tanto no podra usar cartas en este turno.";
                textoAt.GetComponent<TextMeshProUGUI>().fontSize = 18;
                at.transform.SetParent(panel.transform, false);
                textoAt.transform.SetParent(panel.transform, false);
            }
            if (pc.apuntado)
            {
                GameObject ap = new GameObject("ap", typeof(RectTransform));
                ap.AddComponent<Image>().sprite = mirillaS;
                ap.GetComponent<Image>().SetNativeSize();
                GameObject textoAp = new("textoAp", typeof(RectTransform));
                textoAp.AddComponent<TextMeshProUGUI>().text = "El personaje tiene esta apuntado por un francotirador, sino lo matas recibiras daño al final del turno\n";
                textoAp.GetComponent<TextMeshProUGUI>().fontSize = 18;
                ap.transform.SetParent(panel.transform, false);
                textoAp.transform.SetParent(panel.transform, false);
            }
        }
        else
        {
            nombre.text = personaje.GetComponent<DisplayEnemy>().GetName();
            sprite.sprite = personaje.GetComponent<SpriteRenderer>().sprite;
            EnemyController ec = personaje.GetComponent<EnemyController>();
            DisplayEnemy dc = personaje.GetComponent<DisplayEnemy>();
            GameObject texto = new();
            texto.AddComponent<TextMeshProUGUI>().text = "El enemigo hace " + dc.GetDaño() + " de daño y tiene " + dc.enemy.vida + " de vida.\n";
            texto.GetComponent<TextMeshProUGUI>().fontSize = 9;
            LayoutElement le = texto.AddComponent<LayoutElement>();
            le.preferredWidth = 150;
            texto.transform.SetParent(panel.transform,false);
            if (ec.danoFuego > 0)
            {
                GameObject fuego = new("Fuego", typeof(RectTransform));
                fuego.AddComponent<Image>().sprite = fuegoS;
                fuego.GetComponent<Image>().SetNativeSize();
                GameObject textoF = new("textoF", typeof(RectTransform));
                textoF.AddComponent<TextMeshProUGUI>().text = "El personaje tiene " + ec.danoFuego + " cargas de fuego, recibira daño equivalente a las cargas al final del turno y el contador disminuirá en uno.\n";
                textoF.GetComponent<TextMeshProUGUI>().fontSize = 9;
                LayoutElement leF = textoF.AddComponent<LayoutElement>();
                leF.preferredWidth = 150;
                fuego.transform.SetParent(panel.transform, false);
                textoF.transform.SetParent(panel.transform, false);
            }
            if (ec.shock)
            {
                GameObject at = new("at", typeof(RectTransform));
                at.AddComponent<Image>().sprite = aturdidoS;
                at.GetComponent<Image>().SetNativeSize();
                at.transform.localScale = new((float)0.7,(float)0.7,(float)0.7);
                GameObject textoAt = new("textoAt", typeof(RectTransform));
                textoAt.AddComponent<TextMeshProUGUI>().text = "El personaje esta aturdido, y por tanto no podra usar cartas en este turno.\n";
                textoAt.GetComponent<TextMeshProUGUI>().fontSize = 9;
                LayoutElement leAt = textoAt.AddComponent<LayoutElement>();
                leAt.preferredWidth = 150;
                at.transform.SetParent(panel.transform, false);
                textoAt.transform.SetParent(panel.transform, false);
            }
        }
    }
}
