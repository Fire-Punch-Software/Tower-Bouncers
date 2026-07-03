using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] Vector3 direction = Vector3.right;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.y > 1f || transform.position.y < -1f || transform.position.x > 1.435f || transform.position.x < -1.435f)
        {
            Destroy(gameObject);
        }
    }
}
