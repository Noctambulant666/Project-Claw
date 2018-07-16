using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCollectable : MonoBehaviour {
    float speed = 2f;

	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime * speed);

    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            EventManager.TriggerEvent ("coinpickup")
        }
    }
}
