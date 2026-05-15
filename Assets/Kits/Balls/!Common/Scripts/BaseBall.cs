using UnityEngine;

public class BaseBall : MonoBehaviour
{
    [SerializeField] float speed = 5f;
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
            DoImpulse(1f);
        }
    }

    private void FixedUpdate()
    {
        rb2d.linearVelocity = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * speed, rb2d.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡Bola tocó al jugador!");
            Destroy(gameObject);

            DoSplit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShot"))
        {
            Debug.Log("¡Bola tocó al disparo!");
            Destroy(collision.gameObject);

            DoSplit();
        }
    }

    private void DoSplit()
    {
        if (transform.localScale.x > minScaleToSplit)
        {
            if (spawnPointLeft != null)
            {
                CreateBall(spawnPointLeft, -1f);
            }

            if (spawnPointRight != null)
            {
                CreateBall(spawnPointRight, 1f);
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
        rb2d.linearVelocity = new Vector2(horizontalDirection, 1f).normalized * speed;
        impulsed = true;
    }
}