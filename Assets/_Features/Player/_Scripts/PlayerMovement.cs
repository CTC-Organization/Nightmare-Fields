using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Config")]

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;
    [SerializeField] private TrailRenderer tr;

    private Vector2 _movement;
    private Vector2 _currentVelocity;

    AudioSource audioSource;
    private bool hasGun = false;
    float x;

    Animator anim;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;
    private Vector2 input;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private Rigidbody2D _rb;


    [Header("Game Config")]
    private Health health;
    public float currentHealth = 100f;


    private Material material;   
    private Shader defaultShader; 
    private Shader flashShader;
    private Coroutine flashRoutine;
    // shader pode ser melhorada futuramente
    private void Awake()
    {
        health = GetComponent<Health>();
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material; // Obt�m o material do SpriteRenderer
        defaultShader = material.shader;
        flashShader = Shader.Find("GUI/Text Shader"); // shader que consegue piscar
    }

    void Update()
    {
        ProccessInputs();
        Animate();
        if (input.x < 0 && facingLeft || input.x > 0 && !facingLeft)
        {
            Flip();
        }

        if (isDashing)
        {
            return;
        }

       if (Input.GetKeyDown(KeyCode.F))
    {
        hasGun = !hasGun;
        anim.SetBool("HasGun", hasGun); // Atualiza o parâmetro do Animator
        Debug.Log(hasGun);
    }
        // if (Input.GetKeyUp(KeyCode.F))
        // {
        //     isRunning = false;
        // }

        // Atualiza o parâmetro do Animator
        // anim.SetBool("IsRunning", isRunning);

        x = Input.GetAxis("Horizontal") * _moveSpeed;
        _rb.linearVelocity = new Vector2(x, _rb.linearVelocity.y);

        if (_rb.linearVelocity.x != 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        // Movimentação normal
        _movement.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_movement.magnitude > 0)
        {
            _currentVelocity = Vector2.Lerp(_currentVelocity, _movement.normalized * _moveSpeed, _acceleration * Time.deltaTime);
            _rb.linearVelocity = _currentVelocity;
        }
        else
        {
            _currentVelocity = Vector2.Lerp(_currentVelocity, Vector2.zero, _deceleration * Time.deltaTime);
            _rb.linearVelocity = _currentVelocity;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void ProccessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();
    }

    void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        // Ignora colisão com a layer "Enemy"
        toggleEnemyLayerCollision(true);

        if (health.invulnerableCoroutine != null) StopCoroutine(health.invulnerableCoroutine);
        health.invulnerableCoroutine = StartCoroutine(health.InvulnerabilityCoroutine(dashingTime));



        // Salva gravidade atual
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;

        // Determina a direção do dash
        Vector2 dashDirection = _movement.normalized;

        // Se o jogador não está se movendo, usa a direção do eixo X
        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(transform.localScale.x, 0f);
        }

        // Aplica a velocidade do dash
        _rb.linearVelocity = dashDirection * dashingPower;

        // Ativa o efeito visual
        tr.emitting = true;


        yield return new WaitForSeconds(dashingTime);

        // Reseta o movimento e o efeito visual
        _rb.linearVelocity = Vector2.zero;
        tr.emitting = false;
        _rb.gravityScale = originalGravity;

        toggleEnemyLayerCollision(false);
        isDashing = false;

        // Espera pelo cooldown
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void toggleEnemyLayerCollision(bool toggle)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), toggle);
    }

    public void PlayerFlash()
    {
        if (flashRoutine == null)
            flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        int flashes = 3;          // Quantidade de piscadas
        float interval = 0.5f;    // Intervalo entre as piscadas (meio segundo)

        for (int i = 0; i < flashes; i++)
        {
            // Troca para o shader que "acende"
            material.shader = flashShader;
            yield return new WaitForSeconds(interval / 2);

            // Retorna ao shader padr�o
            material.shader = defaultShader;
            yield return new WaitForSeconds(interval / 2);
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





