using UnityEngine;
using System.Collections.Generic;

public class CameraObstacleHandler : MonoBehaviour
{
    public Transform player; // Посилання на гравця
    public LayerMask obstacleLayer; // Шар перешкод
    public float transparency = 0.3f; // Рівень прозорості
    private List<Renderer> previousObstacles = new List<Renderer>();

    void Update()
    {
        HandleObstacles();
    }

    void HandleObstacles()
    {
        // Очистити попередні перешкоди
        foreach (var renderer in previousObstacles)
        {
            SetTransparency(renderer, 1f);
        }
        previousObstacles.Clear();

        // Створюємо Raycast
        Vector3 direction = player.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, obstacleLayer);

        // Обробляємо перешкоди
        foreach (var hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                SetTransparency(renderer, transparency);
                previousObstacles.Add(renderer);
            }
        }
    }

    void SetTransparency(Renderer renderer, float alpha)
    {
        foreach (var material in renderer.materials)
        {
            Color color = material.color;
            color.a = alpha;
            material.color = color;

            // Змінюємо режим рендерингу (для прозорості)
            if (alpha < 1f)
            {
                material.SetFloat("_Mode", 3); // Transparent
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
            else
            {
                material.SetFloat("_Mode", 0); // Opaque
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
            }
        }
    }
}
