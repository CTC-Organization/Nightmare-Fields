using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Config")]

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;
    private Vector2 _movement;
    private Vector2 _currentVelocity;
    [SerializeField] private TrailRenderer tr;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private Rigidbody2D _rb;


    [Header("Game Config")]
    public float currentHealth = 100f;

     // shader pode ser melhorada futuramente


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDashing)
        {
            return;
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

      private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        // Ignora colisão com a layer "Enemy"
        toggleEnemyLayerCollision(true);
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

    private void toggleEnemyLayerCollision(bool toggle) {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), toggle);
    }


}