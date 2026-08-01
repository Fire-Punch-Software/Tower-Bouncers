using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance;

    [Header("Room doors")]
    [SerializeField] private GameObject door;

    private int enemiesAlive = 0;
    private bool doorOpened = false;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy()
    {
        //Debug.Log("Enemy registered");
        enemiesAlive++;
        //Debug.Log(enemiesAlive);
    }

    public void EnemyDied()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
        //Debug.Log(enemiesAlive);

        if (!doorOpened && enemiesAlive == 0)
        {
            doorOpened = true;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        door.SetActive(false);
    }
}
