using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletLife = 1.0f;
    public float rotation = 0f;
    public float speed = 1f;

    private Vector3 spawnPoint;
    private float timer;

    void Start() {
        spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update() {
        if (timer > bulletLife) {
            Destroy(this.gameObject);
        } else {
            timer += Time.deltaTime;
            MoveBullet();
        }
    }

    private void MoveBullet() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
