/// <summary>
/// This MoveForward class will make an object move forward.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {
    public int speed;

    /// <summary>
    /// This method is called before the first frame update.
    /// </summary>
    void Start() {
        
    }
    /// <summary>
    /// This method is called one per frame.
    /// </summary>
    void Update() {
        transform.Translate( Vector3.forward * speed * Time.deltaTime);
    }
}
