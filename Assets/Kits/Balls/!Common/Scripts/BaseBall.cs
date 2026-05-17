using UnityEngine;

public class BaseBall : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float speed = 0.5f;
    [SerializeField] float direction = 1f;
    [SerializeField] float baseGravity = 0.65f;
    [SerializeField] float maxFallSpeed = 6f;
    [SerializeField] float fallSpeedMultiplier = 0.6f;
    //[SerializeField] float minBounceY = 0.5f;
    //[SerializeField] float maxBounceY = 3f;

    [Header("Spawning")]
    [SerializeField] GameObject spawnPointLeft = null;
    [SerializeField] GameObject spawnPointRight = null;
    [SerializeField] GameObject ballPrefab = null;
    [SerializeField] float smallerScale = 0.5f;
    [SerializeField] float minScaleToSplit = 0.3f;

    Rigidbody2D rb2d;
    bool impulsed = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShot"))
        {
            Destroy(collision.gameObject);

            DoSplit();
        }

        if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            DoSplit();
        }
    }

    private void DoSplit()
    {
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

        Destroy(gameObject);
    }

    private void CreateBall(GameObject spawnPoint, float horizontalDirection)
    {
        if (ballPrefab != null)
        {
            GameObject newBall = Instantiate(ballPrefab, spawnPoint.transform.position, Quaternion.identity);
            newBall.transform.localScale = transform.localScale * smallerScale;
            newBall.GetComponent<BaseBall>().DoImpulse(horizontalDirection);
        }
    }

    public void DoImpulse(float horizontalDirection)
    {
        rb2d.linearVelocity = new Vector2(horizontalDirection, speed).normalized * speed;
        impulsed = true;
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