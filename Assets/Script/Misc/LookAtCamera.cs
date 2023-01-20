using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera _camera;
    [SerializeField]
    private Vector2 limitScale;
    [SerializeField]
    private float scalingFactor;

    private void Start()
    {
        _camera = GameContext.instance.gameMasterObject.GetComponent<Camera>();
        if (!_camera)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        transform.LookAt(_camera.transform);
        transform.localScale = Mathf.Clamp(1 + Vector3.Distance(transform.position, _camera.transform.position) / scalingFactor, limitScale.x, limitScale.y) * Vector3.one;
    }

}
