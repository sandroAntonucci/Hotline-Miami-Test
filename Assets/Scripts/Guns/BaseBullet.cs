using System.Collections;
using TMPro.Examples;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // The bullet's movement speed
    public float speed = 5f;
    public float lifeDuration = 2f;

    public BulletPool bulletPool;

    public ParticleSystem hitEffectOne;
    public ParticleSystem hitEffectTwo;

    private Rigidbody rb;

    private Coroutine DestroyCoroutine;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();


        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

        // Draws raycast from the camera to know where the bullet is going
        Transform cameraHolder = GameObject.FindGameObjectWithTag("MainCamera").transform;

        Ray ray = new Ray(cameraHolder.position + cameraHolder.forward * 0.1f, cameraHolder.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit) && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>().isAiming)
        {

            // The direction of the bullet is the hit point minus the bullet's position
            Vector3 direction = (hit.point - transform.position).normalized;

            // The bullet's velocity is the direction multiplied by the speed
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = transform.right * -1 * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            if (DestroyCoroutine != null) return;

            BulletHoleDecalPool.Instance.SpawnBulletHole(collision.contacts[0].point, collision.contacts[0].normal);

            DestroyCoroutine = StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        rb.velocity *= 0.1f;

        hitEffectOne.transform.parent = null;
        hitEffectOne.Play();

        hitEffectTwo.transform.parent = null;
        hitEffectTwo.Play();

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(lifeDuration);
        DestroyCoroutine = null;

        hitEffectOne.transform.parent = gameObject.transform;
        hitEffectOne.transform.localPosition = Vector3.zero;

        hitEffectTwo.transform.parent = gameObject.transform;
        hitEffectTwo.transform.localPosition = Vector3.zero;


        ReturnToPool();
    }

    private void ReturnToPool()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
