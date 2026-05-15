using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BaseCharacter
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference shoot;

    [SerializeField] Transform feet;
    [SerializeField] float groundDistance = 0.05f;
    [SerializeField] LayerMask jumpable;
    [SerializeField] bool hasDoubleJump = false;

    [Header("Configuración de Disparo")]
    [SerializeField] private int maxProjectiles = 1;

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

        jump.action.Enable();
        jump.action.performed += OnJump;

        shoot.action.Enable();
        shoot.action.performed += OnShoot;

    }

    private int activeProjectiles = 0;
    protected override void Update()
    {
        base.Update();

        Move(rawMove);

        activeProjectiles = GameObject.FindGameObjectsWithTag("PlayerShot").Length;
    }

    private void OnDisable()
    {
        move.action.Disable();
        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        jump.action.Disable();
        jump.action.performed -= OnJump;

        shoot.action.Disable();
        shoot.action.performed -= OnShoot;
    }

    private bool OnTheGround()
    {
        return Physics2D.Raycast(feet.position, Vector3.down, groundDistance, jumpable);
    }

    Vector2 rawMove;
    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();
    }

    private bool canDoubleJump = false;
    private void OnJump(InputAction.CallbackContext context)
    {
        if (OnTheGround() || canDoubleJump)
        {
            mustJump = true;

            if (hasDoubleJump)
            {
                canDoubleJump = !canDoubleJump;
            }
        }
    }

    private bool CanShoot()
    {
        return activeProjectiles < maxProjectiles;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (CanShoot() && OnTheGround())
        {
            mustShoot = true;
        }
    }

}
