using UnityEngine;

public class BaseBall : MonoBehaviour
{
    [SerializeField] Color spriteColor = Color.clear;

    [Header("Spawning")]
    [SerializeField] GameObject spawnPointLeft = null;
    [SerializeField] GameObject spawnPointRight = null;
    //[SerializeField] GameObject ballPrefab = null;
    [SerializeField] BaseBall ballPrefab = null;
    [SerializeField] float smallerScale = 0.5f;
    [SerializeField] float minScaleToSplit = 0.3f;

    [Header("Physics")]
    [SerializeField] float speed = 0.5f;
    [SerializeField] float direction = 1f;
    [SerializeField] float baseGravity = 0.65f;
    [SerializeField] float maxFallSpeed = 6f;
    [SerializeField] float fallSpeedMultiplier = 0.6f;
    //[SerializeField] float minBounceY = 0.5f;
    //[SerializeField] float maxBounceY = 3f;

    [Header("Bounce Height")]
    [SerializeField]
    BounceConfig[] bounceConfigs = new BounceConfig[]
    {
        new BounceConfig { scale = 0.3625f,   height = 0.582f  }, // la bola grande siempre llega a Y=0.582
        new BounceConfig { scale = 0.18125f,  height = 0.23f   }, // la mediana siempre a Y=0.23
        new BounceConfig { scale = 0.090625f, height = -0.1f   }  // la pequeña a Y=-0.1
    };

    [System.Serializable]
    public struct BounceConfig
    {
        public float scale;
        public float height;
    }

    Rigidbody2D rb2d;
    SpriteRenderer sr;
    bool impulsed = false;

    private void Awake()
    {
        RoomController.Instance.RegisterEnemy();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Color finalColor = spriteColor;

        if (finalColor == Color.clear)
        {
            finalColor = Random.ColorHSV(
                0f, 1f,
                0.6f, 1f,
                0.7f, 1f
            );
        }

        sr.color = finalColor;
    }
    
    private void Start()
    {
        if (!impulsed)
        {
            DoImpulse(direction);
        }
    }

    private void FixedUpdate()
    {
        rb2d.linearVelocity = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * speed, rb2d.linearVelocity.y);

        //Gravity();
    }

    bool isSplitting = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSplitting) return;
        if (collision.gameObject.CompareTag("PlayerShot"))
        {
            isSplitting = true;
            Destroy(collision.gameObject);
            DoSplit();
        }

        if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            isSplitting = true;
            DoSplit();
        }
    }

    private void DoSplit()
    {
        //GetComponent<Collider2D>().enabled = false;

        if (transform.localScale.x > minScaleToSplit)
        {
            if (spawnPointLeft != null)
            {
                CreateBall(spawnPointLeft, -1f * speed);
            }

            if (spawnPointRight != null)
            {
                CreateBall(spawnPointRight, 1f * speed);
            }
        }

        RoomController.Instance.EnemyDied();
        Destroy(gameObject);
    }

    private void CreateBall(GameObject spawnPoint, float horizontalDirection)
    {
        if (ballPrefab != null)
        {
            BaseBall newBall = Instantiate(ballPrefab, spawnPoint.transform.position, Quaternion.identity);
            newBall.transform.localScale = transform.localScale * smallerScale;
            newBall.DoImpulse(horizontalDirection);
        }
    }

    public void DoImpulse(float horizontalDirection)
    {
        float targetY = GetTargetY();
        float currentY = transform.position.y;
        float gravity = Physics2D.gravity.magnitude * rb2d.gravityScale;

        float verticalSpeed = 0f;
        float heightDiff = targetY - currentY;

        if (heightDiff > 0f)
        {
            // La bola está por debajo del objetivo → calcula velocidad para llegar exactamente ahí
            verticalSpeed = Mathf.Sqrt(2f * gravity * heightDiff);
        }
        else
        {
            // La bola está por encima o en el objetivo → impulso mínimo hacia arriba (solo rebota y cae)
            verticalSpeed = 0f; // o un valor pequeño fijo si quieres que siempre suba algo
        }

        rb2d.linearVelocity = new Vector2(horizontalDirection, verticalSpeed);
        impulsed = true;
    }

    private float GetTargetY()
    {
        float currentScale = transform.localScale.x;
        float closestDiff = float.MaxValue;
        float selectedHeight = 0f;

        foreach (BounceConfig config in bounceConfigs)
        {
            float diff = Mathf.Abs(config.scale - currentScale);
            if (diff < closestDiff)
            {
                closestDiff = diff;
                selectedHeight = config.height; // ahora es posición Y en el mundo, no altura relativa
            }
        }

        return selectedHeight;
    }

    private void Gravity()
    {
        if (rb2d.linearVelocity.y < 0)
        {
            rb2d.gravityScale = baseGravity * fallSpeedMultiplier;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, Mathf.Max(rb2d.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb2d.gravityScale = baseGravity;
        }
    }
}