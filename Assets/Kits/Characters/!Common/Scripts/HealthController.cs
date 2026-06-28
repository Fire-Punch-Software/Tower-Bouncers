using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    public void GetDamage(float damage)
    {
        Animator animator = gameObject.GetComponent<Animator>();

        health -= damage;
        if (health <= 0)
        {
            animator.SetBool("IsDead", true);
            Invoke(nameof(DestroyObject), 0.7f);

        }
        else
        {
            animator.SetTrigger("GotDamage");
        }
    }

    private void DestroyObject()
    {
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }
}
