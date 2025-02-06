using UnityEngine;

public class RaycastToShaderGraph : MonoBehaviour
{
    public Camera mainCamera;

    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Renderer renderer = hit.transform.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.SetVector("_hitPoint", hit.point);
                }
            }
        }
    }
}