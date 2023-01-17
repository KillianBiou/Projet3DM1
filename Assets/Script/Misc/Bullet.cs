using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ScheduleDestruction());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 150, Space.Self);
    }

    private IEnumerator ScheduleDestruction()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
