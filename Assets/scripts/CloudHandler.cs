using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHandler : MonoBehaviour {

	void Start() {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (-Random.Range (-0.3f, 0.6f), 0);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag.Equals ("CloudDestructor")) {
			if (GetComponent<Rigidbody2D> ().velocity.x < 0) {
				transform.position = new Vector2 (12, transform.position.y + Random.Range (-2f, 2f));
			} else {
				transform.position = new Vector2 (-12, transform.position.y + Random.Range (-2f, 2f));
			}
		}
	}
}
