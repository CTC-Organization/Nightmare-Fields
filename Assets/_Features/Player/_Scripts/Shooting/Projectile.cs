using UnityEngine;

namespace TopDown.Shooting
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Movement Stats")]
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        private Rigidbody2D body;
        private float lifeTimer;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        public void ShootBullet(Vector2 direction)
        {
            lifeTimer = 0;
            transform.up = direction; // Ajusta a rotação do projétil para a direção do tiro
            gameObject.SetActive(true);

            body.linearVelocity = direction.normalized * speed; // Define a velocidade constante do projétil
        }

        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Obstacle"))
            {
                Destroy (gameObject);
            }
        }
    }
}