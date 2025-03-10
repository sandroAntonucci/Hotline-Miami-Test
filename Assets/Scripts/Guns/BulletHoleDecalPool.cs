using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleDecalPool : MonoBehaviour
{

    public static BulletHoleDecalPool Instance;

    public Sprite[] sprites;
    public GameObject bulletHolePrefab; // Assign a bullet hole prefab in the Inspector
    public int poolSize = 20; // Number of decals to pool
    private Queue<GameObject> bulletHolePool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        bulletHolePool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab);
            bulletHole.SetActive(false);
            bulletHolePool.Enqueue(bulletHole);
        }
    }

    public void SpawnBulletHole(Vector3 position, Vector3 normal)
    {
        GameObject bulletHole = bulletHolePool.Dequeue();
        bulletHole.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        bulletHole.transform.position = position + normal * 0.001f;
        bulletHole.transform.rotation = Quaternion.LookRotation(normal);
        bulletHole.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        bulletHole.SetActive(true);

        StartCoroutine(FadeOutBulletHole(bulletHole));
        bulletHolePool.Enqueue(bulletHole);
    }

    private IEnumerator FadeOutBulletHole(GameObject bulletHole)
    {

        float startScale = bulletHole.transform.localScale.x;

        yield return new WaitForSeconds(5f);

        while (bulletHole.transform.localScale.x > 0)
        {
            bulletHole.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
            yield return null;
        }

        bulletHole.SetActive(false);

        bulletHole.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
    }
}

