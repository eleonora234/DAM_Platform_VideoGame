using UnityEngine;
using UnityEngine.UIElements;

public class camera_follow : MonoBehaviour
{
    [Header("settings")]
    public Vector3 followOffset;

    public float smoothSpeed = 0.2f;






    [Header("components")]
    public Transform playerTransform;
    float zPosition;
    Vector3 currentVelocity = Vector3.zero;

    public void Awake()
    {
        zPosition = transform.position.z;
    }

    public void Update()
    {
        Vector3 targetPosition = playerTransform.position + followOffset;
        targetPosition.z = zPosition;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);
    }
















}
