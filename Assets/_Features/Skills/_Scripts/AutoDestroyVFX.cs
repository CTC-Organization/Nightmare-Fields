using System.Collections;
using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
        [SerializeField] private float lifeTime = 0;
        [SerializeField] private float lifeTimer = 2;
        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }
}