using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    public bool isInvulnerable = false;
    public Coroutine invulnerableCoroutine;
    [SerializeField] private float invulnerabilityDurationOnHit;

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
            Debug.Log($"Caio Took Damage: {_damage}, Current Health: {currentHealth}");

            if (currentHealth > 0)
            {
                //hurt
                Debug.Log("Dano no player por aproximacao");
                if (invulnerableCoroutine != null) StopCoroutine(invulnerableCoroutine);
                PlayerFlash();
                invulnerableCoroutine = StartCoroutine(InvulnerabilityCoroutine(invulnerabilityDurationOnHit));

            }
            else
            {
                //die
                Debug.Log("Player Died");
                SetDying();
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

    public IEnumerator InvulnerabilityCoroutine(float invulnerabilityDuration)
    {
        isInvulnerable = true;
        Debug.Log("Player is now invulnerable.");
        yield return new WaitForSeconds(invulnerabilityDuration);
        Debug.Log("Se passou: " + invulnerabilityDuration + "segundos");

        isInvulnerable = false;
        Debug.Log("Player is no longer invulnerable.");
        invulnerableCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy: " + collision.name);
            takeDamage(1); // dano padrao
        }
    }

    public void PlayerFlash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            material.shader = defaultShader;
        }
        ; ;
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        int flashes = 3;          // Quantidade de piscadas
        float interval = invulnerabilityDurationOnHit / (3 * 2) - 0.01f;    // Intervalo entre as piscadas (meio segundo)

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

    public void TakeEnemyAttackDamage(HitType hitType, float damage) // caso o hitType for HitType.defend, poderia causar dano a alguma propriedade escudo de power up
    {
        switch (hitType)
        {
            case HitType.Hit:
                {
                    Debug.Log("Dano no player por ataque basico do inimigo");

                    // mexe nas rotinas - invencibilidade e flash do player
                    if (invulnerableCoroutine != null) StopCoroutine(invulnerableCoroutine);
                    PlayerFlash();
                    invulnerableCoroutine = StartCoroutine(InvulnerabilityCoroutine(invulnerabilityDurationOnHit));
                    // mexe na vida
                    currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
                    if (currentHealth <= 0) Debug.Log("Player Died"); // Die
                    break;
                }
            default:
                break;
        }
    }
    private void SetDying()
    {
        Debug.Log("Player esta morrendo");
        // PlayerMovement em = GetComponent<PlayerMovement>();
        // em.isDying = true;
        // em.animator.SetBool("IsDying", em.isDying);
        // colocar função Die() após fim da animação de morte...
    }

        public void Die()
    {
        Debug.Log("Inimigo " + name + " morreu");
        Destroy(gameObject); // colocar animação - o final da animação ativa a função de destuir o objeto em destruir
        // essa função deve appenas ativar um boolean isDead e ativar o animator para ativara a animação de morte (no final dispara o evento de destruir)
        GameManager.instance.GameOver();
    
    }
}