using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10f;
    }

    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10f;
    }
}