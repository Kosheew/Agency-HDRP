using UnityEngine;
using TMPro;

public class FadingText : MonoBehaviour
{
    [SerializeField] private float duration = 2f;  // Час анімації
    [SerializeField] private Vector3 startPosition = new Vector3(0, 5, 0);  // Початкова позиція
    [SerializeField] private Vector3 endPosition = new Vector3(0, -5, 0);  // Кінцева позиція

    private TextMeshProUGUI textMesh;
    private float elapsedTime = 0f;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.transform.position = startPosition;
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            // Поступовий рух тексту вниз
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            textMesh.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // Поступове зникнення тексту
            Color color = textMesh.color;
            color.a = Mathf.Lerp(1f, 0f, t);  // Відповідно до часу зменшуємо прозорість
            textMesh.color = color;
        }
        else
        {
          //  Destroy(gameObject);  // Знищити текст після завершення анімації
        }
    }
}