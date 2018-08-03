using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelScreenHandler : MonoBehaviour {

	public GameObject LevelTile;
	public Transform frames;
	public GameObject witch;
	public GameObject chatHead;

	void Start() {
		ArrangeLevelScreen ();
		fillStatus();
	}

	private void fillStatus() {
		int lockedFrom = PlayerPrefs.GetInt("locked from", -1);
		if (lockedFrom == -1) {
			PlayerPrefs.SetInt ("locked from", 2);
			lockedFrom = 2;
		}
		// Change UI elements accordingly
		int noOfBalls = frames.childCount;
		for (int i = 0; i < noOfBalls; i++) {
			if (i+1 >= lockedFrom) {
				frames
					.GetChild (i)
					.GetChild (0)
					.GetComponent<Image> ().color = new Color (75f / 255, 75f / 255, 75f / 255);

				frames.GetChild (i).GetComponent<Animator> ().enabled = false;
				frames.GetChild(i).GetComponent<Button>().onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
				frames.GetChild (i).GetComponent<Button> ().onClick.AddListener (ActivateChatHead);
			}
		}
		// lock the witch level
		if (lockedFrom <= 10) {
			witch.GetComponent<Image>().color = new Color(75f / 255, 75f / 255, 75f / 255);
			witch.GetComponent<Animator> ().enabled = false;
			witch.GetComponent<Button> ().onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
			witch.GetComponent<Button> ().onClick.AddListener (ActivateChatHead);
		}
	}

	private void ActivateChatHead() {
		chatHead.SetActive (true);
		Invoke ("DeactivateChatHead", 2f);
	}

	private void DeactivateChatHead() {
		chatHead.SetActive (false);
	}

	private void ArrangeLevelScreen() {
		string destination = Application.persistentDataPath + "/levels_info.dat";
		FileStream file;

		if (File.Exists (destination)) {
			file = File.OpenRead (destination);
		} else {
			Debug.LogError("File not found");
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		LevelsInfo data = (LevelsInfo) bf.Deserialize(file);

		for (int i = 0; i < frames.childCount; i++) {
			GameObject level = frames.GetChild (i).gameObject;
			if (data.levelContent [i].IndexOf ("q") != -1) {
				level.transform.GetChild (1).GetChild (0).gameObject.SetActive (true);
			}
			if (data.levelContent [i].IndexOf ("h") != -1) {
				level.transform.GetChild (1).GetChild (1).gameObject.SetActive (true);
			}
			if (data.levelContent [i].IndexOf ("w") != -1) {
				level.transform.GetChild (1).GetChild (2).gameObject.SetActive (true);
			}
			if (data.levelContent [i].IndexOf ("te") != -1) {
				level.transform.GetChild (1).GetChild (3).gameObject.SetActive (true);
			}
			if (data.levelContent [i].IndexOf ("r") != -1) {
				level.transform.GetChild (1).GetChild (4).gameObject.SetActive (true);
			}
		}

		//for the witch
		if (data.levelContent [9].IndexOf ("q") != -1) {
			witch.transform.GetChild(0).GetChild (0).gameObject.SetActive (true);
		}
		if (data.levelContent [9].IndexOf ("h") != -1) {
			witch.transform.GetChild(0).GetChild (1).gameObject.SetActive (true);
		}
		if (data.levelContent [9].IndexOf ("w") != -1) {
			witch.transform.GetChild(0).GetChild (2).gameObject.SetActive (true);
		}
		if (data.levelContent [9].IndexOf ("te") != -1) {
			witch.transform.GetChild(0).GetChild (3).gameObject.SetActive (true);
		}
		if (data.levelContent [9].IndexOf ("r") != -1) {
			witch.transform.GetChild(0).GetChild (4).gameObject.SetActive (true);
		}

		file.Close ();
	}

	public void OpenLevel(int level) {
		StateManager.SetLevel (level);
		StateManager.SetIsCustom (false);
		if (level == 10) {
			SceneManager.LoadScene ("PreBossLevel");
		} else {
			LoadingScreenManager.LoadScene (2);
		}
	}

	public void OnClickBack() {
		SceneManager.LoadScene ("main");
	}

	public void OnClickCustomLevels() {
		SceneManager.LoadScene ("custom levels");
	}

}
