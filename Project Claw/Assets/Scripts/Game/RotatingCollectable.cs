using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCollectable : MonoBehaviour {
    float speed = 2f;

	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "Player")
        {
            EventManager.TriggerEvent("coinpickup");
            Destroy(this.gameObject);
            AudioManager.instance.Play("Collect coin");
        }
    }
}
