using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    [Header("Base Stats")]
    [SerializeField] private PlayerBaseStats baseStats;

    // Runtime stats (persist between scenes)
    public float currentMaxHealth;
    public float currentMoveSpeed;
    public float currentFireRate;
    public float currentDamage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeFromBase();
    }

    public void InitializeFromBase()
    {
        currentMaxHealth = baseStats.maxHealth;
        currentMoveSpeed = baseStats.moveSpeed;
        currentFireRate = baseStats.fireRate;
        currentDamage = baseStats.damage;
    }

    public void EndRun()
    {
        InitializeFromBase();
    }
}