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
            body.linearVelocity = Vector2.zero;
            transform.up = direction; // Ajusta a rotação do projétil para a direção do tiro
            gameObject.SetActive(true);

            body.linearVelocity = direction * speed; // Define a velocidade constante do projétil
        }

        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}