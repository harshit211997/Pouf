using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour {

	public float dampTime = 1f;
	private Vector3 velocity = new Vector3(10, 10,0);
	public Transform target;
	public GameManager manager;

	private Vector3 targetVector;
	private Vector3 position;

	void Start() {
		position = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		targetVector.x = transform.position.x;
		targetVector.y = manager.groundY;
		Vector3 point = GetComponent<Camera>().WorldToViewportPoint(targetVector);
		Vector3 delta = targetVector - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.4f, point.z)); //(new Vector3(0.5, 0.5, point.z));
		Vector3 destination = transform.position + delta;
		position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		position.x = target.position.x + 5;
		transform.position = position;
	}
}
