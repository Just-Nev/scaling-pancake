using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    [SerializeField] AudioClip hitSound;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Play sound on hit:
                // Get hit location
                Vector2 hitPoint = collision.GetContact(0).point;
                
                // Create temp GameObject at hit location
                GameObject tempSoundGameObject = new GameObject("tempSoundGameObject");
                tempSoundGameObject.transform.position = hitPoint;
                
                // Add AudioSource and play sound from temp GameObject
                AudioSource tempAudioSource = tempSoundGameObject.AddComponent<AudioSource>();
                tempAudioSource.clip = hitSound;
                tempAudioSource.volume = 0.15f;
                tempAudioSource.pitch = Random.Range(0.8f, 1.2f);
                tempAudioSource.Play();
                
                // Delete temp GameObject
                Destroy(tempSoundGameObject, hitSound.length);

            // Add points based on ScoreManager settings
            ScoreManager.Instance.AddPoints(ScoreManager.Instance.pointsPerAsteroid);

        }
    }
}





