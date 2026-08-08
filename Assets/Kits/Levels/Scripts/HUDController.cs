using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject healthLevel;
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

    private float fullHealthWidth = 6.7f;
    private void UpdateHealth(float hp, float maxHp)
    {
        currentHealth.text = $"{hp:0}/{maxHp:0}";

        float healthPercentage = (hp / maxHp) * 100;
        healthPercentage = healthPercentage * fullHealthWidth / 100;
        healthLevel.transform.localScale = new Vector3(healthPercentage, 1, 1);
    }

    private void UpdateScore(int score)
    {
        currentScore.text = $"{score:0}";
    }
}