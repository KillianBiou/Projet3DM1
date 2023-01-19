using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

    [SerializeField]
    private Transform target;

    private void Update()
    {
        transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}
