using System;
using UnityEngine;

public class RunState : MonoBehaviour
{
    public static RunState Instance;

    [Header("Player health")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float hp = 100f;

    [Header("Game score")]
    [SerializeField] private int currentScore = 0;

    public float HP => hp;
    public float MaxHp => maxHp;
    public int CurrentScore => currentScore;

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnScoreChanged;

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
    void Start()
    {
        OnHealthChanged?.Invoke(hp, maxHp);
        OnScoreChanged?.Invoke(currentScore);
    }

    public void TakeDamage(float amount)
    {
        hp = Mathf.Max(0, hp - amount);
        OnHealthChanged?.Invoke(hp, maxHp);
    }

    public void GetHeal(float amount)
    {
        hp = Mathf.Min(maxHp, hp + amount);
        OnHealthChanged?.Invoke(hp, maxHp);
    }

    public void GetScore(int score)
    {
        // Implementation for getting score
        currentScore += score;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void ResetRun()
    {
        hp = maxHp;
        currentScore = 0;
        OnHealthChanged?.Invoke(hp, maxHp);
        OnScoreChanged?.Invoke(currentScore);
    }

    public bool IsDead()
    {
        return hp <= 0;
    }
}
