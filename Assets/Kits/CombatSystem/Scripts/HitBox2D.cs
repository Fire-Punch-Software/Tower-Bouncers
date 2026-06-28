using UnityEngine;

public class HitBox2D : MonoBehaviour
{
    [SerializeField] private string affectedTag = "Enemy";
    [SerializeField] public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) 
        {
            Debug.LogWarning("Collision null en HitBox2D");
            return;
        }
        
        GameObject obj = collision.gameObject;
        if (obj == null) return;
        
        if (collision.CompareTag(affectedTag))
        {
            Debug.Log(affectedTag);

            // Intenta MovementController primero
            if (collision.TryGetComponent<BaseCharacter>(out var movement))
            {
                movement.NotifyHit(this);
                Destroy(gameObject);
            }
            // Luego ItemController
            //else if (collision.TryGetComponent<ItemController>(out var item))
            //{
                //item.NotifyHit(this);
            //}
        }
    }

}
