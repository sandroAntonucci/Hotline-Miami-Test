using TMPro.Examples;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // The bullet's movement speed
    public float speed = 5f;

    public BulletPool bulletPool;

    private Rigidbody rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * -1 * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(collision.gameObject.name);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
