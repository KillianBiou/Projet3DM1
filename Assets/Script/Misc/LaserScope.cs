using UnityEngine;
using System.Collections;
using TMPro;

public class LaserScope : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    private float laserMaxLength = 5f;
    private bool canPreview = true;

    private void Start()
    {
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;
    }

    void LateUpdate()
    {
        if(canPreview)
            ShootLaserFromTargetPosition(transform.position, transform.forward, laserMaxLength);
    }

    public void SetCanPreview(bool canPreview)
    {
        if (canPreview)
        {
            canPreview = true;
            laserLineRenderer.enabled = true;
        }
        else
        {
            canPreview = false;
            laserLineRenderer.enabled = false;
        }
    }

    public void SetLaserMaxLenght(float lenght)
    {
        laserMaxLength = lenght;
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }
}