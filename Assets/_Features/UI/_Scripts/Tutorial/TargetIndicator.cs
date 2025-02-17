using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    public Transform target = null;  // O objeto no mundo real que será apontado
    public RectTransform arrow;  // A seta que vai girar
    public RectTransform circle;  // A seta que vai girar
    public RectTransform enemyAnimated;  // A seta que vai girar
    public RectTransform indicatorGroup;  // O grupo de UI (zumbi, círculo e seta)
    public Camera mainCamera;  // A câmera principal
    public bool activated = false;

    void Start()
    {
        mainCamera = Camera.main;
        arrow.gameObject.SetActive(false);
        circle.gameObject.SetActive(false);
        enemyAnimated.gameObject.SetActive(false);
    }
    void Update()
    {
        if (activated == true && target == null)
        {
            arrow.gameObject.SetActive(true);
            circle.gameObject.SetActive(true);
            enemyAnimated.gameObject.SetActive(true);
        }
        else if (activated == true && target != null)
        {



            // Converte a posição do alvo do mundo para a tela
            Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(target.position);

            // Verifica se o alvo está dentro da tela
            bool isOnScreen = targetScreenPos.z > 0 &&
                              targetScreenPos.x > 0 && targetScreenPos.x < Screen.width &&
                              targetScreenPos.y > 0 && targetScreenPos.y < Screen.height;

            if (isOnScreen)
            {
                // Se o alvo está na tela, o indicador segue ele diretamente
                indicatorGroup.position = targetScreenPos;
            }
            else
            {
                // Se o alvo está fora da tela, posicionamos o indicador na borda

                Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                Vector3 dir = (targetScreenPos - screenCenter).normalized;

                // Calcula a posição na borda da tela
                float borderX = Mathf.Clamp(targetScreenPos.x, 50, Screen.width - 50);
                float borderY = Mathf.Clamp(targetScreenPos.y, 50, Screen.height - 50);
                Vector3 clampedPosition = new Vector3(borderX, borderY, 0);

                indicatorGroup.position = clampedPosition;
            }

            // Gira a seta para sempre apontar para o alvo
            Vector3 direction = targetScreenPos - indicatorGroup.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
