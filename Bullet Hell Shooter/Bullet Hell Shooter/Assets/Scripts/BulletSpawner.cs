using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Circle, Spiral, Spin }
    public TimeManager timeManager;
    public bool isRunningCircle = false;
    public bool isRunningSpiral = false;
    public bool isRunningSpin = false;

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 75.0f;
    private float bulletSpeed = 25.0f;
    private int maxBulletsToSpawn = 250;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType = SpawnerType.Circle;
    [SerializeField] private float firingRate = 0.125f;
    private int bulletCount = 0;
    public BulletCountUI bulletCountUI;

    private int bulletsToSpawnPerInterval = 1;
    private float spawnerSpeed = 20.0f;
    private Vector3 initialPosition;

    private GameObject spawnedBullet;
    private float timer = 15f;
    private float currentRotation = 0f;
    private float rotationIncrement = 0.90f;
    private int totalRotations = 0;

    private void Start()
    {
        StartCoroutine(StartPatternSequence());
    }

    void Update() 
    {
        timer += Time.deltaTime;

        if (timer >= firingRate) {
            Fire();
            timer = 0f;
        }

    }

    private IEnumerator StartPatternSequence()
    {
        yield return new WaitForSeconds(0.001f);
        StartCoroutine(CirclePattern());
    }

    private IEnumerator CirclePattern()
    {
        isRunningCircle = true;

        while (isRunningCircle)
        {
            spawnerType = SpawnerType.Circle;
            transform.Rotate(Vector3.up * rotationIncrement);
            currentRotation += rotationIncrement;

            if (currentRotation >= 720f)
            {
                currentRotation = 0f;
                totalRotations++;

                float startTime = Time.time;
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = transform.position - Vector3.down * 20.0f;

                while (Time.time - startTime <= (10.0f / spawnerSpeed))
                {
                    float t = (Time.time - startTime) / (10.0f / spawnerSpeed);
                    transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                    yield return null;
                }

                transform.position = targetPosition;
                isRunningCircle = false;

                // Agregamos una pausa antes de cambiar al siguiente patrón
                yield return new WaitForSeconds(1f);

                // Cambiamos al siguiente patrón
                StartCoroutine(SpiralPattern());
            }

            // Agregamos una pequeña pausa entre cada frame de la corrutina
            yield return null;
        }
    }

    private IEnumerator SpiralPattern()
    {
        isRunningSpiral = true;

        while (isRunningSpiral && TimeManager.Minute > 11  && TimeManager.Minute < 21)
        {
            float radius = 7.5f;
            float angularSpeed = 0.85f;

            float x = radius * Mathf.Sin(angularSpeed * Time.time);
            float y = 1.25f * radius * Mathf.Sin(2 * angularSpeed * Time.time);

            transform.position = new Vector3(x, y, transform.position.z);

            // Agregamos una pequeña pausa entre cada frame de la corrutina
            yield return null;
        }

        isRunningSpiral = false;
        StartCoroutine(SpinPattern());
    }

    private IEnumerator SpinPattern()
    {
        isRunningSpin = true;

        while (isRunningSpin )
        {
            transform.eulerAngles += new Vector3(0f, 0f, 2f);
            transform.Rotate(Vector3.up * rotationIncrement);
            currentRotation += rotationIncrement;

            if (currentRotation >= 149f)
            {
                currentRotation = 0f;
                totalRotations++;
                spawnerType = SpawnerType.Circle;
            }

            // Agregamos una pequeña pausa entre cada frame de la corrutina
            yield return null;
        }

        isRunningSpin = false;
    }

    private IEnumerator RotateToInitialRotation()
    {
        float startTime = Time.time;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(90f, 180f, 0f);

        while (Time.time - startTime <= 0.175f)
        {
            float t = (Time.time - startTime) / 2.0f;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Agregamos una pequeña pausa entre cada frame de la corrutina
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private void Fire()
    {
        if (bullet)
        {
            int bulletsToSpawn = Mathf.Min(maxBulletsToSpawn - bulletCount, bulletsToSpawnPerInterval);
            for (int i = 0; i < bulletsToSpawn; i++)
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
                spawnedBullet.GetComponent<Bullet>().rotation = transform.eulerAngles.z;
                bulletCount++;
            }
            Debug.Log("Bullets Spawned: " + bulletCount);
        }

        if (bulletCountUI != null)
        {
            bulletCountUI.UpdateBulletCount(bulletCount);
        }
    }
}
