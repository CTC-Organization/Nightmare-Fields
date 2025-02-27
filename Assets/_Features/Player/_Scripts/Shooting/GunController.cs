using TopDown.Shooting;
using Unity.Cinemachine;
using UnityEngine;

namespace TopDown.Shooting
{
    public class GunController : MonoBehaviour
    {
        //Cooldown normal attack
        [Header("Cooldown normal attack")]
        [SerializeField] private float coolDown = 0.25f;
        private float _coolDownTimer;

        [Header("References")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _bulletPrefabEspecial;
        [SerializeField] private Transform _firePoint;

        //Cooldown especial attack
        [Header("Cooldown especial attack")]
        [SerializeField] private float coolDownSpecial = 5f;
        private float _coolDownTimerEspecial;

        private void Update()
        {
            if (GameManager.instance.isOnFarm) return;

            _coolDownTimer += Time.deltaTime;
            _coolDownTimerEspecial += Time.deltaTime;
            if (InputManager.IsShooting)
            {
                Shoot();
            }
            if (InputManager.IsSpecialAttacking)
            {
                SpecialAttack();
            }
        }

        private void SpecialAttack()
        {
            if (_coolDownTimerEspecial < coolDownSpecial) return;

            // Obter a posi��o do mouse e ajustar a posi��o Z para corresponder ao plano de jogo

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            mousePosition.z = 0; // Garantir que a posi��o Z seja 0 para o plano 2D

            // Calcular a dire��o do tiro
            Vector2 direction = (mousePosition - _firePoint.position).normalized;
            GameObject bullet2 = Instantiate(_bulletPrefabEspecial, _firePoint.position, Quaternion.identity);
            bullet2.GetComponent<EspecialProjectile>().ShootEspecialBullet(direction);

            Debug.Log("Special Attack!");
            _coolDownTimerEspecial = 0;
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