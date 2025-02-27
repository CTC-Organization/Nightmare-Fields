using UnityEngine;
using System.Collections;
using System.Linq;
using Pathfinding;

public class GargoyleAttack : MonoBehaviour
{
    [SerializeField] private int damage = 0;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float attackStartDistance;
    private Coroutine attackRoutine = null;

    private IAstarAI ai;

    [SerializeField] private EnemyController ec;

    private float lastAttackTime;
    private GameObject target;
    [SerializeField] private GameObject curvedProjectilePrefab;
    [SerializeField] private GameObject straightProjectilePrefab;
    [SerializeField] private float burstFireInterval = 0.1f;
    [SerializeField] private int burstFireCount = 5;


    private void Start()
    {
        ai = GetComponent<IAstarAI>();
        ec = GetComponent<EnemyController>();
        target = GameObject.FindWithTag("Player");
    }

    private void Update()
    {

        if (ec.isDying)
        {
            if (attackRoutine != null) StopAllCoroutines();
            return;
        }
        else if (ai.reachedDestination && target != null && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            attackRoutine = StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        Vector3 attackOrigin = transform.position + directionToTarget * attackStartDistance;

        int sorteio = Random.Range(0, 100);
        // if (sorteio > 66)
        // {       // tiro com trajetória inclinada

        //     GameObject curvedProjectile = Instantiate(curvedProjectilePrefab, attackOrigin, Quaternion.identity);
        //     curvedProjectile.GetComponent<CurvedProjectile>().Shoot(directionToTarget, target.transform.position);
        // }
        if (sorteio > 50)
        { // tiro com trajetória reta
            yield return StartCoroutine(BurstFire(directionToTarget, attackOrigin));
        }
        else
        { // tiro com trajetória reta
            yield return StartCoroutine(GuidedBurstFire());
        }

        yield return new WaitForSeconds(attackCooldown);
    }

    private IEnumerator BurstFire(Vector3 direction, Vector3 origin)
    {
        for (int i = 0; i < burstFireCount; i++)
        {
            // Instancia o tiro reto
            GameObject straightProjectile = Instantiate(straightProjectilePrefab, origin, Quaternion.identity);
            straightProjectile.GetComponent<StraightProjectile>().Shoot(direction);

            // Espera o intervalo entre tiros
            yield return new WaitForSeconds(burstFireInterval / burstFireCount);
        }
    }
    private IEnumerator GuidedBurstFire()
    {
        for (int i = 0; i < burstFireCount; i++)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            Vector3 attackOrigin = transform.position + directionToTarget * attackStartDistance;

            GameObject straightProjectile = Instantiate(straightProjectilePrefab, attackOrigin, Quaternion.identity);
            straightProjectile.GetComponent<StraightProjectile>().Shoot(directionToTarget);

            // Espera o intervalo entre tiros
            yield return new WaitForSeconds(burstFireInterval / burstFireCount);
        }
    }
}


