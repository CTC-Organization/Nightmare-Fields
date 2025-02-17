using UnityEngine;

public class DestroyAfterVFX : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float lifeTimer = 2f;
    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
