using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public bool IsShooting = false;
    public Animator anim; // Referência ao componente Animator
    // public AudioSource audioSource; // Referência ao componente AudioSource
    // public AudioClip shootSound; // Som de tiro

    private Coroutine resetCoroutine; // Armazena a referência da corrotina atual

    void Update()
    {
        // Verifica se o botão esquerdo do mouse foi pressionado
        if (Input.GetMouseButtonDown(0))
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine); // Para a corrotina anterior se existir
            }

            IsShooting = true;
            Debug.Log("Atirando!");

            // Atualiza o parâmetro do Animator
            anim.SetBool("IsShooting", IsShooting);

            // Toca o som de tiro
            // if (audioSource != null && shootSound != null)
            // {
            //     audioSource.PlayOneShot(shootSound);
            // }

            // Inicia a corrotina para resetar IsShooting após um delay
            resetCoroutine = StartCoroutine(ResetShootingAfterDelay());
        }
    }

    // Corrotina para resetar IsShooting após um delay
    IEnumerator ResetShootingAfterDelay()
    {
        // Define o tempo de delay (0.5 segundos)
        float delay = 0.5f;
        yield return new WaitForSeconds(delay); // Aguarda o delay

        // Após o delay, define IsShooting como false
        IsShooting = false;
        Debug.Log("Parou de atirar.");

        // Atualiza o parâmetro do Animator
        anim.SetBool("IsShooting", IsShooting);
    }
}