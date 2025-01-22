using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float flashRoutineDuration = 2f;
    public float currentHealth { get; private set; }
    private bool isInvulnerable = false;
    private Coroutine invulnerableCoroutine;

    [Header("VFX Config")]
    [Tooltip("Material para tratar shader")]
    [SerializeField]
    private Material material;      // Material associado ao SpriteRenderer
    [SerializeField]
    [Tooltip("Salva shader original caso se perca")] private Shader defaultShader;  // Shader padr�o do material
    [SerializeField]
    [Tooltip("Shader que simula piscada ao levar hit")] private Shader flashShader;
    [Tooltip("Guarda rotina de piscadas por segundo ao ser hitado")] private Coroutine flashRoutine;


    private void Awake()
    {
        currentHealth = startingHealth;
        Debug.Log($"Initial Health: {currentHealth}");
    }

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material; // Obt�m o material do SpriteRenderer
        defaultShader = material.shader;
        flashShader = Shader.Find("GUI/Text Shader"); // shader que consegue piscar
    }

    private void takeDamage(float _damage)
    {
        if (!isInvulnerable)
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            Debug.Log($"Took Damage: {_damage}, Current Health: {currentHealth}");

            if (currentHealth > 0)
            {
                //hurt
                Debug.Log("Player Hurt");
                if (invulnerableCoroutine != null) StopCoroutine(invulnerableCoroutine);
                invulnerableCoroutine = StartCoroutine(InvulnerabilityCoroutine());

            }
            else
            {
                //die
                Debug.Log("Player Died");
            }
        }
        else
        {
            Debug.Log("Player is invulnerable and did not take damage.");
        }
    }

    private void Update()
    {
        //losing life
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Key 'E' Pressed");
            takeDamage(1);
        }
        //adding life
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Key 'P' Pressed");
            AddHealth(1);
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        Debug.Log($"Healed: {_value}, Current Health: {currentHealth}");
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        PlayerFlash();
        isInvulnerable = true;
        Debug.Log("Player is now invulnerable.");
        yield return new WaitForSeconds(flashRoutineDuration);
        isInvulnerable = false;
        Debug.Log("Player is no longer invulnerable.");
        invulnerableCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy: " + collision.name);
            takeDamage(1);
        }
    }

    public void PlayerFlash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            material.shader = defaultShader;
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        int flashes = 3;          // Quantidade de piscadas
        float interval = flashRoutineDuration / (3 * 2) - 0.01f;    // Intervalo entre as piscadas (meio segundo)

        for (int i = 0; i < flashes; i++)
        {
            // Troca para o shader que "acende"
            material.shader = flashShader;
            yield return new WaitForSeconds(interval);

            // Retorna ao shader padr�o
            material.shader = defaultShader;
            yield return new WaitForSeconds(interval);
        }
    }

    public void TakeEnemyAttackDamage(HitType hitType, float damage)
    {


        switch (hitType)
        {
            case HitType.Hit:
                {

                    currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
                    PlayerFlash(); // flash pisca pisca do player
                    if (currentHealth <= 0) Debug.Log("Player Died"); // Die
                    break;
                }
            default:
                break;
        }
    }

}