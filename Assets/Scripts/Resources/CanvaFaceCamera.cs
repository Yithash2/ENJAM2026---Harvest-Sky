using UnityEngine;

public class CanvaFaceCamera : MonoBehaviour
{
    void Update()
    {
        Transform camPos = Camera.main.transform;
        transform.LookAt(transform.position + camPos.rotation * Vector3.forward, camPos.rotation * Vector3.up);
    }
}
