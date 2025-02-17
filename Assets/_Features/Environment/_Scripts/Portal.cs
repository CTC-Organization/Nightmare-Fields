using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string sceneToLoadName;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;
        GameManager.instance.tm.Teleport(sceneToLoadName);
    }
}
