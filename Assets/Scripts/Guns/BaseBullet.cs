using System.Collections;
using JetBrains.Annotations;
using TMPro.Examples;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // The bullet's movement speed
    public float speed = 5f;
    public float lifeDuration = 2f;
    public int bulletDamage;

    public bool canDamage = true;

    public bool isEnemyBullet = false;

    public BulletPool bulletPool;

    public ParticleSystem hitEffectOne;
    public ParticleSystem hitEffectTwo;

    private Rigidbody rb;

    private Coroutine DestroyCoroutine;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        canDamage = true;


        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

        // Draws raycast from the camera to know where the bullet is going
        Transform cameraHolder = GameObject.FindGameObjectWithTag("MainCamera").transform;


        Ray ray = new Ray(cameraHolder.position + cameraHolder.forward * 0.1f, cameraHolder.forward);
        RaycastHit hit;


        if (isEnemyBullet)
        {
            Vector3 playerDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
            rb.velocity = playerDirection * speed * 0.5f;
        }

        else
        {
            if (Physics.Raycast(ray, out hit) && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>().isAiming)
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                rb.velocity = direction * speed;
            }
            else
            {
                rb.velocity = transform.right * -1 * speed;
            }
        }

    }

    private void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit, moveDistance * 1000) && !isEnemyBullet)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                AIHandler aiHandlerComponent = hit.collider.GetComponent<AIHandler>();
                if (aiHandlerComponent != null)
                {
                    aiHandlerComponent.DealDamage(bulletDamage);
                    ReturnToPool();
                }
            }
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (DestroyCoroutine != null) return;

            BulletHoleDecalPool.Instance.SpawnBulletHole(collision.contacts[0].point, collision.contacts[0].normal);

            canDamage = false;

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
