using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Themes {
	GRASSY, VOLCANO, DUSK, GOTHIC, NIGHT
};

public class ThemeManager : MonoBehaviour {

	public GameObject[] quarterObs;
	public GameObject[] halfObs;
	public GameObject[] eightObs;
	public GameObject[] wholeObs;
	public GameObject[] restObs;
	public GameObject[] platform;
	public GameObject[] finalPlatform;
	public Sprite[] background;
	public AudioClip[] backgroundSound;
	public float[] musicbarPos;
	public float[] scaleBackground;

}
