using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickIndicator : MonoBehaviour {

	public void ChangeColor(Color color) {
		GetComponent<SpriteRenderer> ().color = color;
	}

	public Color GetColor() {
		return GetComponent<SpriteRenderer> ().color;
	}
}
