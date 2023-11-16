using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

    enum SpawnerType { Straight, Spin }
    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1.0f;
    public float speed = 0.75f;
    private float distanceTraveled = 0f;
    private float maxDistance = 20f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;

    private GameObject spawnedBullet;
    private float timer = 0f;

    void Start() {
        
    }

    void Update() {
        timer += Time.deltaTime;

        if (spawnerType == SpawnerType.Straight) {
            float distanceToMove = speed * Time.deltaTime;
            if (distanceTraveled + distanceToMove <= maxDistance) {
                transform.Translate(Vector3.forward * distanceToMove);
                distanceTraveled += distanceToMove;
            }
        }

        if (spawnerType == SpawnerType.Spin) transform.eulerAngles += new Vector3(0f, 0f, 2f);

        if (timer >= firingRate) {
            Fire();
            timer = 0f;
        }
    }

    private void Fire() {
        if (bullet) {
            spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().rotation = transform.eulerAngles.z;
        }
    }
}
