using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public bool isDying = false;


    public void UpdateAnimatorParameters(Vector3 velocity)
    {
        animator.SetFloat("moveSpeed", velocity.magnitude);

        if (velocity.magnitude > 0.1f)
        {
            animator.SetFloat("moveX", velocity.x);
            animator.SetFloat("moveY", velocity.y);
        }
    }
}
