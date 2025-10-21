using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
