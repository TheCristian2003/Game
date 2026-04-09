using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;   // personaje
    public float distance = 5f;

    public float mouseSensitivity = 200f;
    public float minY = -30f;
    public float maxY = 60f;

    float rotationX = 0f;
    float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // bloquea el mouse
    }

    void LateUpdate()
    {
        // Movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationY += mouseX;
        rotationX -= mouseY;

        // Limitar rotación vertical
        rotationX = Mathf.Clamp(rotationX, minY, maxY);

        // Rotación final
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Posición de la cámara
        Vector3 position = target.position - rotation * Vector3.forward * distance;

        transform.position = position;
        transform.LookAt(target);
    }
}