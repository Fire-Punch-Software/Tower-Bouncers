using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BaseCharacter
{
    [Header("HUD")]
    [SerializeField] TextMeshProUGUI currentHealth;
    [SerializeField] TextMeshProUGUI currentPowerLevel;
    [SerializeField] TextMeshProUGUI currentLevel;
    [SerializeField] TextMeshProUGUI gameOver;

    [Header("Player movement")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference shoot;
    [SerializeField] InputActionReference bomb;

    [Header("Player physics")]
    [SerializeField] Transform feet;
    [SerializeField] float groundDistance = 0.05f;
    [SerializeField] LayerMask jumpable;
    [SerializeField] bool hasDoubleJump = false;

    [Header("Player shooting")]
    [SerializeField] private int maxProjectiles = 1;
    [SerializeField] GameObject secondarySpawnPoint = null;
    [SerializeField] GameObject secondaryProjectilePrefab = null;
    [SerializeField] private int secondaryMaxProjectiles = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        currentHealth.text = RunState.Instance.hp.ToString();
    }

    public void Refresh()
    {
        currentHealth.text = RunState.Instance.hp.ToString();
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

        bomb.action.Enable();
        bomb.action.performed += OnBomb;
    }

    private int activeProjectiles = 0;
    private bool mustBomb = false;
    protected override void Update()
    {
        base.Update();

        Move(rawMove);

        activeProjectiles = GameObject.FindGameObjectsWithTag("PlayerShot").Length;

        if (mustBomb && secondarySpawnPoint)
        {
            mustBomb = false;
            animator.SetTrigger("PerformBomb");

            GameObject grenade = Instantiate(secondaryProjectilePrefab, secondarySpawnPoint.transform.position, Quaternion.identity);

            float dirX = transform.localScale.x > 0 ? 1f : -1f;
            grenade.GetComponent<Grenade>().SetDirection(new Vector3(dirX, 0f, 0f));
        }

    }

    protected override void Move(Vector2 direction)
    {
        bool isShooting = animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Bomb");

        if (!isShooting)
        {
            float linearSpeed = GetLinearSpeed();
            rb2d.position += lastMoveDirection * linearSpeed * Time.deltaTime;
            lastMoveDirection = direction;
        }
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

        bomb.action.Disable();
        bomb.action.performed -= OnBomb;
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

    private bool CanBomb()
    {
        return activeProjectiles < secondaryMaxProjectiles;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (CanShoot() && OnTheGround())
        {
            mustShoot = true;
        }
    }

    private void OnBomb(InputAction.CallbackContext context)
    {
        if (CanBomb())
        {
            mustBomb = true;
        }
    }

    public override void NotifyHit(HitBox2D hitBox2D)
    {
        base.NotifyHit(hitBox2D);

        RunState.Instance.TakeDamage(hitBox2D.damage);

        currentHealth.text = RunState.Instance.hp.ToString();
    }


    protected override void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }
}
