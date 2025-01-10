using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;
    public static bool IsShooting;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _shootAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _shootAction = _playerInput.actions["Shoot"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
        IsShooting = _shootAction.triggered;
    }
}