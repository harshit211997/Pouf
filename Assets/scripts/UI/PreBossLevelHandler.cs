using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreBossLevelHandler : MonoBehaviour {

	void Start () {
		Invoke ("StartBossLevel", 2f);
	}

	private void StartBossLevel() {
		SceneManager.LoadScene ("game");
	}
}