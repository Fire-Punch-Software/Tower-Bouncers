using UnityEngine;

public class RunState : MonoBehaviour
{
    public static RunState Instance;

    public float maxHp = 100f;
    public float hp = 100f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TakeDamage(float amount)
    {
        hp = Mathf.Max(0, hp - amount);
    }

    public void Heal(float amount)
    {
        hp = Mathf.Min(maxHp, hp + amount);
    }

    public bool IsDead()
    {
        return hp <= 0;
    }
}
