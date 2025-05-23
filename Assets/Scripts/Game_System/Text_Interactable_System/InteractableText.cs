using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractableText : MonoBehaviour
{
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private Image textImage;
    private Coroutine coroutine;
    
    private void Awake()
    {
        textImage.fillAmount = 0f;  
    }

    public void OpenInteractableText()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);  
            
        coroutine = StartCoroutine(StartAnimation(0f, 1f)); 
        
    }

    public void CloseInteractableText()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);  
            
        coroutine = StartCoroutine(StartAnimation(textImage.fillAmount, 0f));
    }

    private IEnumerator StartAnimation(float startFill, float endFill)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < animationDuration)
        {
            textImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsedTime / animationDuration);  
            elapsedTime += Time.deltaTime;  
            yield return null;  
        }
        
        textImage.fillAmount = endFill;
    }
}
