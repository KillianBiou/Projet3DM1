using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(GetComponent<Rigidbody>());
        }
    }

    public IEnumerator ScheduleDestruction(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        for(int i = 100; i > 0; i--)
        {
            this.transform.localScale = Vector3.one * (i / 100f);
            yield return new WaitForNextFrameUnit();
        }

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
