using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BaseCharacter
{
    [SerializeField] InputActionReference move;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        move.action.Enable();
        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;
    }

    protected override void Update()
    {
        base.Update();

        Move(rawMove);
    }

    private void OnDisable()
    {
        move.action.Disable();
        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
    }

    Vector2 rawMove;
    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();
    }
}
