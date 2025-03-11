using UnityEngine;

public class AimRaycast : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private float distance;
    [SerializeField] private ParticleSystem particle;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.05f;
        
    }

    void Update()
    {
       lineRenderer.SetPosition(0, transform.position);
       var ray = new Ray(transform.position, transform.forward);

       if (Physics.Raycast(ray, out RaycastHit hit))
       {
           if (hit.collider)
           {
               lineRenderer.SetPosition(1, hit.point);
               particle.transform.position = hit.point;
           }
       }
       else
       {
           lineRenderer.SetPosition(1, transform.forward * distance);
       }
    }
}