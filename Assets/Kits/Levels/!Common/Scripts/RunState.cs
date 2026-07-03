using UnityEngine;

public class RunState : MonoBehaviour
{
    public static RunState Instance;

    public int maxHp = 100;
    public int hp = 100;

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
}
