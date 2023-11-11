using System.Collections;
using UnityEngine;

public class Triangle : MonoBehaviour {
    public Vector3 startPosition = new Vector3(-100f, 109f, 0);
    public Vector3 endPosition = new Vector3(1200f, 109f, 0);
    public float jumpDistance = 10000000000000.0f;
    public float jumpInterval = 0.001f;

    private bool shouldMove = false;

    void OnEnable() {
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    void OnDisable() {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }

    void TimeCheck() {
        if (TimeManager.Hour == 0 && TimeManager.Minute == 5) {
            shouldMove = true;
            StartCoroutine(MoveTriangle());
        }
    }

    IEnumerator MoveTriangle() {
        while (shouldMove) {
            float remainingDistance = Vector3.Distance(transform.position, endPosition);

            while (remainingDistance > jumpDistance) {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, jumpDistance*160);
                yield return new WaitForSeconds(jumpInterval);  
                remainingDistance = Vector3.Distance(transform.position, endPosition);
            }

            transform.position = endPosition;
            shouldMove = false;

            yield return null;
        }
    }
}
