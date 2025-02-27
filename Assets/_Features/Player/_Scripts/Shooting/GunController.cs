using TopDown.Shooting;
using Unity.Cinemachine;
using UnityEngine;

namespace TopDown.Shooting
{
    public class GunController : MonoBehaviour
    {
        //Cooldown
        [Header("Cooldown")]
        [SerializeField] private float coolDown = 0.25f;
        private float _coolDownTimer;

        [Header("References")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _firePoint;

        private void Update()
        {
            _coolDownTimer += Time.deltaTime;

            if (InputManager.IsShooting)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (_coolDownTimer < coolDown) return;

            // Obter a posi��o do mouse e ajustar a posi��o Z para corresponder ao plano de jogo
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            mousePosition.z = 0; // Garantir que a posi��o Z seja 0 para o plano 2D

            // Calcular a dire��o do tiro
            Vector2 direction = (mousePosition - _firePoint.position).normalized;

            Debug.Log($"Mouse Position: {mousePosition}");
            Debug.Log($"Fire Point Position: {_firePoint.position}");
            Debug.Log($"Direction: {direction}");

            // Screenshake
            CinemachineImpulseSource cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
            cinemachineImpulseSource.GenerateImpulse();
            // Criar o proj�til
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
            bullet.GetComponent<Projectile>().ShootBullet(direction);

            _coolDownTimer = 0f; // Reiniciar o cooldown
        }

    }
}