using UnityEngine;

public class UFOBoss : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 90f; // degrees per second

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public Transform[] firePoints;

    private float fireTimer;

    void Update()
    {
        RotateObject();
        HandleShooting();
    }

    void RotateObject()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    void HandleShooting()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        if (bulletPrefab == null || firePoints.Length == 0)
            return;

        foreach (Transform fp in firePoints)
        {
            Instantiate(bulletPrefab, fp.position, fp.rotation);
        }
    }
}
