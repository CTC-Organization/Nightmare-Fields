using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimenta��o")]

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;
    private Vector2 _movement;
    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;


    [Header("Configura��es de L�gica do jogo")]
    public float currentHealth = 100f;

    [Header("Configura��es de VFX")]
    [Tooltip("Material para tratar shader")]
    [SerializeField]
    private Material material;      // Material associado ao SpriteRenderer
    [SerializeField]
    [Tooltip("Salva shader original caso se perca")] private Shader defaultShader;  // Shader padr�o do material
    [SerializeField]
    [Tooltip("Shader que simula piscada ao levar hit")] private Shader flashShader;
    [Tooltip("Guarda rotina de piscadas por segundo ao ser hitado")] private Coroutine flashRoutine;
    // shader pode ser melhorada futuramente


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material; // Obt�m o material do SpriteRenderer
        defaultShader = material.shader;
        flashShader = Shader.Find("GUI/Text Shader"); // shader que consegue piscar
    }

    // Update is called once per frame
    void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        if (_movement.magnitude > 0)
        {
            _currentVelocity = Vector2.Lerp(_currentVelocity, _movement * _moveSpeed, _acceleration * Time.deltaTime);
        }
        else
        {
            _currentVelocity = Vector2.Lerp(_currentVelocity, Vector2.zero, _deceleration * Time.deltaTime);
        }

        _rb.linearVelocity = _currentVelocity;
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
        float interval = 1f;    // Intervalo entre as piscadas (meio segundo)

        for (int i = 0; i < flashes; i++)
        {
            // Troca para o shader que "acende"
            material.shader = flashShader;
            yield return new WaitForSeconds((interval * interval / flashes / 2) - 0.01f);

            // Retorna ao shader padr�o
            material.shader = defaultShader;
            yield return new WaitForSeconds((interval * interval / flashes / 2) - 0.01f);
        }
    }

    public void TakeDamage(HitType hitType, float damage)
    {
        switch (hitType)
        {
            case HitType.Hit:
                {
                    currentHealth -= damage;
                    PlayerFlash(); // flash pisca pisca do player
                }
                break;
            default:
                break;
        }
    }



}