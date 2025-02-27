using UnityEngine;

namespace TopDown.Shooting
{
    public class GunController : MonoBehaviour
    {
        [Header("Cooldown normal attack")]
        [SerializeField] private float coolDown = 0.25f;
        private float _coolDownTimer;

        [Header("References")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _bulletPrefabEspecial;
        [SerializeField] private Transform _firePoint;

        [Header("Cooldown especial attack")]
        [SerializeField] private float coolDownSpecial = 5f;
        private float _coolDownTimerEspecial;

        private bool isTripleShotActive = false;
        private bool isAutoFireActive = false;
        private float powerUpTimer = 0f;

        private void Update()
        {
            if (GameManager.instance.isOnFarm) return;

            _coolDownTimer += Time.deltaTime;
            _coolDownTimerEspecial += Time.deltaTime;
            powerUpTimer -= Time.deltaTime;

            // Desativa o power-up quando o tempo acabar
            if (powerUpTimer <= 0)
            {
                isTripleShotActive = false;
                isAutoFireActive = false;
            }

            if (InputManager.IsShooting && (!isAutoFireActive || _coolDownTimer >= coolDown))
            {
                Shoot();
            }

            if (InputManager.IsSpecialAttacking)
            {
                SpecialAttack();
            }
        }

        public void ActivatePowerUp(PowerUp.PowerUpType type, float duration)
        {
            powerUpTimer = duration; // Resetamos o timer antes de ativar o efeito

            if (type == PowerUp.PowerUpType.TripleShot)
            {
                isTripleShotActive = true;
            }
            else if (type == PowerUp.PowerUpType.AutoFire)
            {
                isAutoFireActive = true;
            }
        }

        private void Shoot()
        {
            if (_coolDownTimer < coolDown) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            mousePosition.z = 0;

            Vector2 direction = (mousePosition - _firePoint.position).normalized;

            if (isTripleShotActive)
            {
                ShootTriple(direction);
            }
            else
            {
                SpawnBullet(direction);
            }

            _coolDownTimer = 0f;
        }

        private void ShootTriple(Vector2 direction)
        {
            float angleOffset = 15f;
            Quaternion leftRotation = Quaternion.Euler(0, 0, angleOffset);
            Quaternion rightRotation = Quaternion.Euler(0, 0, -angleOffset);

            SpawnBullet(direction);
            SpawnBullet(leftRotation * direction);
            SpawnBullet(rightRotation * direction);
        }

        private void SpawnBullet(Vector2 direction)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
            bullet.GetComponent<Projectile>().ShootBullet(direction);
        }

        private void SpecialAttack()
        {
            if (_coolDownTimerEspecial < coolDownSpecial) return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            mousePosition.z = 0;

            Vector2 direction = (mousePosition - _firePoint.position).normalized;
            GameObject bullet = Instantiate(_bulletPrefabEspecial, _firePoint.position, Quaternion.identity);
            bullet.GetComponent<EspecialProjectile>().ShootEspecialBullet(direction);

            _coolDownTimerEspecial = 0;
        }
    }
}
