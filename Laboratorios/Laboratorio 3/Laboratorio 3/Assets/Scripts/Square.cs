using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    public void OnEnable() {
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    public void OnDisable() {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }

    private void TimeCheck() {
        // Cada 10 minutos
        if(TimeManager.Hour == 10 && TimeManager.Minute == 00)
        {
            StartCoroutine(MoveSquare());
        }
    }

    private IEnumerator MoveSquare() {
        transform.position = new Vector3(-100f,109f,0);
        Vector3 targetPos = new Vector3(1200f,109f,0);

        Vector3 currentPos = transform.position;

        float timeElapsed = 0;
        float timeToMove = 3;

        while(timeElapsed < timeToMove){
            transform.position = Vector3.Lerp(currentPos,targetPos,timeElapsed/timeToMove);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }
}
