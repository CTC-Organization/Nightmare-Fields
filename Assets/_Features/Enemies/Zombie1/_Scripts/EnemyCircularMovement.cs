using UnityEngine;
using Pathfinding;
using UnityEditor;
using System.Security.Cryptography;

public class EnemyCircularMovement : MonoBehaviour
{
    private Transform target;
    private Vector3 fixedPosition;
    private float radius;
    private float shrinkSpeed;

    private float minRadius;
    private float startRadius;

    private EnemyHealth eh;
    private EnemyController ec;
    private Vector3 previousPosition;
    private Vector3 velocity;

    public void Initialize(Transform target, float startRadius, float shrinkSpeed, float minRadius)
    {
        this.target = target;
        this.startRadius = startRadius;
        radius = startRadius;
        this.shrinkSpeed = shrinkSpeed;
        this.minRadius = minRadius;
        previousPosition = transform.position;
        fixedPosition = target.position;
    }
    void Start()
    {
        ec = GetComponent<EnemyController>();
        eh = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        UpdateCircle();
    }

    void UpdateCircle()
    {
        if (target == null) return;

        // Reduz o raio gradualmente
        radius -= shrinkSpeed * Time.deltaTime;
        if (radius == startRadius / 2) shrinkSpeed += shrinkSpeed * 2;
        else if (radius == startRadius / 3) shrinkSpeed += shrinkSpeed * 2;


        // Calcula nova posição no círculo
        Vector3 direction = (transform.position - fixedPosition).normalized;
        Vector3 newPosition = fixedPosition + direction * radius;

        // Calcula a velocidade com base na posição anterior e na nova posição
        velocity = (newPosition - previousPosition) / Time.deltaTime;
        previousPosition = newPosition;

        // Atualiza a posição do inimigo
        transform.position = newPosition;

        // Se o raio atingir o mínimo, destruir o inimigo
        if (radius <= minRadius)
        {
            eh.TakeDamage(eh.currentHealth);
        }
        ec.UpdateAnimatorParameters(velocity);
    }
}
