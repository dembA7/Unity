using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        //Mover vehiculo hacia adelante
        //transform.Translate(0,0,1);
        transform.Translate(Vector3.forward);
    }
}
