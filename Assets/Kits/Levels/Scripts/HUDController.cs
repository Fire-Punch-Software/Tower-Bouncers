using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHealth;
    [SerializeField] private TextMeshProUGUI currentScore;

    private void OnEnable()
    {
        if (RunState.Instance != null)
        {
            RunState.Instance.OnHealthChanged += UpdateHealth;
            RunState.Instance.OnScoreChanged += UpdateScore;
            UpdateHealth(RunState.Instance.HP, RunState.Instance.MaxHp);
            UpdateScore(RunState.Instance.CurrentScore);
        }
    }

    private void OnDisable()
    {
        if (RunState.Instance != null)
        {
            RunState.Instance.OnHealthChanged -= UpdateHealth;
            RunState.Instance.OnScoreChanged -= UpdateScore;
        }
    }

    private void UpdateHealth(float hp, float maxHp)
    {
        currentHealth.text = $"{hp:0}";
    }

    private void UpdateScore(int score)
    {
        currentScore.text = $"{score:0}";
    }
}