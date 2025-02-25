using System.Collections;
using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
        [SerializeField] private float lifeTime = 2;
        [SerializeField] private float lifeTimer = 0;
        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }
}