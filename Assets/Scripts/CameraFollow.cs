using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // tu personaje
    public Vector3 offset;        // posición relativa
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}
