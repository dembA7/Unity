using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

    enum SpawnerType { Circle, Spin }
    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 35.0f;
    public float bulletSpeed = 25.0f;
    private int maxBulletsToSpawn = 250;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 0.125f;
    private float distanceToMove = 20.0f;
    private int bulletsSpawned = 0;
    private int bulletsToSpawnPerInterval = 1;
    private float spawnerSpeed = 25.0f;

    private GameObject spawnedBullet;
    private float timer = 15f;
    private float currentRotation = 0f;
    private float rotationIncrement = 1.25f;
    private int totalRotations = 0;
    private int maxRotations = 2;

    void Update() {
        timer += Time.deltaTime;

        if (spawnerType == SpawnerType.Circle) {
            RotateAround();
        }

        if (spawnerType == SpawnerType.Spin) transform.eulerAngles += new Vector3(0f, 0f, 2f);

        if (timer >= firingRate) {
            Fire();
            timer = 0f;
        }

        if (totalRotations >= maxRotations) {
            StartCoroutine(MoveShip());
        }
    }

    private IEnumerator MoveShip() {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position - Vector3.down * distanceToMove;

        while (Time.time - startTime <= (distanceToMove / spawnerSpeed)) {
            float t = (Time.time - startTime) / (distanceToMove / spawnerSpeed);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        enabled = false;
    }

    private void Fire() {
        if (bullet) {
            int bulletsToSpawn = Mathf.Min(maxBulletsToSpawn - bulletsSpawned, bulletsToSpawnPerInterval);
            for (int i = 0; i < bulletsToSpawn; i++) {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().speed = bulletSpeed;
                spawnedBullet.GetComponent<Bullet>().rotation = transform.eulerAngles.z;
                bulletsSpawned++;
            }
            Debug.Log("Bullets Spawned: " + bulletsSpawned);
        }
    }

    private void RotateAround() {
        transform.Rotate(Vector3.up * rotationIncrement);
        currentRotation += rotationIncrement;

        if (currentRotation >= 360f) {
            currentRotation = 0f;
            totalRotations++;

            if (totalRotations >= maxRotations) {
                enabled = false; 
            }
        }
    }
}
