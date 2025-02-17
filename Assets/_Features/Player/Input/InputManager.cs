using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;
    public static bool IsShooting;
    public static bool IsSpecialAttacking;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _shootAction;
    private InputAction _specialAttackAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _shootAction = _playerInput.actions["Shoot"];
        _specialAttackAction = _playerInput.actions["SpecialAttack"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
        IsShooting = _shootAction.triggered;
        IsSpecialAttacking = _specialAttackAction.triggered;
    }
}