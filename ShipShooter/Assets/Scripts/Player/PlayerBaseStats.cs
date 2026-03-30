using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/Player Base Stats")]
public class PlayerBaseStats : ScriptableObject
{
    [Header("Core Stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public float damage = 1f;
    public float money = 0f;
    public int reroll = 1;
}