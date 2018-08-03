using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

	public AnimationCurve motion;

	private float timeElapsed;
	private float initialPosY;
	private Vector3 position;
	private enum State {
		deactivate, moving
	};
	private State current;

	void Start () {
		timeElapsed = 0;
		current = State.deactivate;
		initialPosY = transform.localPosition.y;
	}
	
	void Update () {
		switch (current) {
		case State.moving:
			timeElapsed += Time.deltaTime;

			position.y = initialPosY + motion.Evaluate (timeElapsed);
			transform.localPosition = position;
			break;

		case State.deactivate:
			break;
		}
	}

	public void StartMoving() {
		current = State.moving;
	}

}
