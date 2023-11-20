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
    public float bulletLife = 99999999.0f;
    private float bulletSpeed = 25.0f;
    private int maxBulletsToSpawn = 999999;

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
    private float rotationIncrement = 0.80f;
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
        Debug.Log("Circle Pattern Initiated");

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
                Debug.Log("Circle Pattern Terminated");

                yield return new WaitForSeconds(1f);
                StartCoroutine(SpinPattern());
            }

            yield return null;
        }
    }

    private IEnumerator SpiralPattern()
    {
        isRunningSpiral = true;
        Debug.Log("Spiral Pattern Initiated");

        while (isRunningSpiral)
        {
            spawnerType = SpawnerType.Spiral;
            float radius = 7.5f;
            float angularSpeed = 0.85f;
            float startTime = Time.time;

            while (Time.time - startTime <= 7.5f)
            {
                float x = radius * Mathf.Sin(angularSpeed * Time.time);
                float y = 1.25f * radius * Mathf.Sin(1.75f * angularSpeed * Time.time);
                transform.position = new Vector3(x, y, transform.position.z);
                yield return null;
            }

            isRunningSpiral = false;
            Debug.Log("Spiral Pattern Terminated");

        }

        yield return null;
    }

    private IEnumerator SpinPattern()
    {
        isRunningSpin = true;
        Debug.Log("Spin Pattern Initiated");

        while (isRunningSpin)
        {
            spawnerType = SpawnerType.Spin;
            float startTime = Time.time;
            Vector3 startPosition = transform.position;

            while (Time.time - startTime <= 8.6f)
            {
                transform.eulerAngles += new Vector3(0f, 0f, 2f);
                transform.Rotate(Vector3.up * rotationIncrement);
                currentRotation += rotationIncrement;  

                if (currentRotation >= 420f)
                {
                    currentRotation = 0f;
                }

                yield return null;
            }

            isRunningSpin = false;
            Debug.Log("Spin Pattern Terminated");

            yield return new WaitForSeconds(1f);
            StartCoroutine(RotateToInitialRotation());

            yield return new WaitForSeconds(1f);
            if (Time.time - startTime >= 5.0f)
            {
                Vector3 targetPosition = transform.position - Vector3.up * 20.0f;   
                float t = (Time.time - startTime) / (20.0f / spawnerSpeed);
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }

            yield return new WaitForSeconds(1f);
            StartCoroutine(SpiralPattern());
        }
    }

    private IEnumerator RotateToInitialRotation()
    {
        float startTime = Time.time;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(90f, 180f, 0f);

        while (Time.time - startTime <= 0.235f)
        {
            float t = (Time.time - startTime) / 2.0f;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

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
        }

        if (bulletCountUI != null)
        {
            bulletCountUI.UpdateBulletCount(bulletCount);
        }
    }
}
