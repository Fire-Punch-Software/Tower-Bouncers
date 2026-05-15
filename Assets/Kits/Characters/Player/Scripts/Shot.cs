using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] Vector3 direction = Vector3.right;
    
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.y > 3f)
        {
            Destroy(gameObject);
        }
    }
}
