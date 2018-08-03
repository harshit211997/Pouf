using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflaterManager : MonoBehaviour {

	public AnimationCurve expand, contract;

	private enum State {
		STILL, EXPANDING, CONTRACTING
	};
	private State current;
	private float time;
	private Vector3 scale;
	private SpriteRenderer coverSprite;

	void Start () {
		current = State.STILL;
		time = 0;
		coverSprite = transform.GetChild (2).GetComponent<SpriteRenderer> ();
	}
	
	void Update () {
		switch(current) {
		case State.EXPANDING:
			time += Time.deltaTime;
			scale.x = expand.Evaluate (time);
			scale.y = scale.x;
			transform.localScale = scale;
			break;
		case State.CONTRACTING:
			time += Time.deltaTime;
			scale.x = contract.Evaluate (time);
			scale.y = scale.x;
			transform.localScale = scale;
			break;

		case State.STILL:

			break;
		}
	}

	public void StartExpanding() {
		time = 0;
		current = State.EXPANDING;
	}

	public void StartContracting() {
		time = 0;
		current = State.CONTRACTING;
	}

	public void ChangeColor(Color color) {
		coverSprite.color = color;
	}

}
