using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] public float health = 100f;

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

    public void GetHeal(float heal)
    {
        health += heal;
        if (health > 100f)
        {
            health = 100f;
        }
    }

    public float GetHealth()
    {
        return this.health;
    }

    private void DestroyObject()
    {
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }
}
