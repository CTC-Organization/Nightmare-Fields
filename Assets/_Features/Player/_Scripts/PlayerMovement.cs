using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;

    private Vector2 _movement;
    private Vector2 _currentVelocity;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
}