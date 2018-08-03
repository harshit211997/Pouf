using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class CustomLevelScreenHandler : MonoBehaviour {

	public GameObject LevelTile;
	public GameObject normalElements, deleteConfirmPanel;
	public GameObject noLevelsText;

	private Transform ScrollPanel;

	void Start() {
		ScrollPanel = transform.GetChild(1).GetChild (0).GetChild(0);
		ArrangeCustomLevels ();
	}

	private void ArrangeCustomLevels() {
		string destination = Application.persistentDataPath + "/custom_levels_info.dat";
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter();

		if (File.Exists (destination)) {
			file = File.OpenRead (destination);
			LevelsInfo data = (LevelsInfo)bf.Deserialize (file);

			int size = data.levelContent.Count;
			for (int i = 0; i < size; i++) {
				CreateLevelTitle (i + 1, GetContent (data.levelContent [i]));
			}

			file.Close ();

			//Now write the modified content
			file = File.OpenWrite (destination);
			bf.Serialize (file, data);
		} else {
			noLevelsText.SetActive (true);
		}
	}

	private string GetContent(string levelString) {
		string o = "";
		if (levelString.IndexOf ("q") != -1) {
			o += "q ";
		} 
		if (levelString.IndexOf ("h") != -1) {
			o += "h ";
		}
		if (levelString.IndexOf ("te") != -1) {
			o += "te ";
		}
		if (levelString.IndexOf ("w") != -1) {
			o += "w ";
		}
		if (levelString.IndexOf ("r") != -1) {
			o += "r ";
		}
		return o;
	}

	private void CreateLevelTitle(int l, string levelString) {
		GameObject level = GameObject.Instantiate (LevelTile);
		level.transform.parent = ScrollPanel;
		level.transform.localScale = Vector3.one;

		level.transform.GetChild (0).GetComponent<Text> ().text = l.ToString ();

		Transform horizontal = level.transform.GetChild (1);
		if (levelString.IndexOf ("q") != -1) {
			horizontal.GetChild (0).gameObject.SetActive (true);
		}
		if (levelString.IndexOf ("h") != -1) {
			horizontal.GetChild (1).gameObject.SetActive (true);
		}
		if (levelString.IndexOf ("w") != -1) {
			horizontal.GetChild (2).gameObject.SetActive (true);
		}
		if (levelString.IndexOf ("te") != -1) {
			horizontal.GetChild (3).gameObject.SetActive (true);
		}
		if (levelString.IndexOf ("r") != -1) {
			horizontal.GetChild (4).gameObject.SetActive (true);
		}

		level.GetComponent<Button> ().onClick.AddListener (delegate {OpenCustomLevel(l); });
	}

	private void OpenCustomLevel(int level) {
		StateManager.SetIsCustom (true);
		StateManager.SetLevel (level);
		LoadingScreenManager.LoadScene (2);
	}

	public void OnClickCreateCustomLevel() {
		SceneManager.LoadScene ("Custom Level Creator");
	}

	public void OnClickBack() {
		SceneManager.LoadScene ("levels 1");
	}

	public void OnClickDelete() {
		deleteConfirmPanel.SetActive (true);
		normalElements.SetActive (false);
	}

	public void OnConfirmDelete() {
		DeleteAll ();
		normalElements.SetActive (true);
		deleteConfirmPanel.SetActive (false);
		SceneManager.LoadScene ("custom levels");
	}

	public void OnCancelDelete() {
		normalElements.SetActive (true);
		deleteConfirmPanel.SetActive (false);
	}

	public void DeleteAll() {
		// Delete levels info file
		string destination = Application.persistentDataPath + "/custom_levels_info.dat";
		if (File.Exists (destination)) {
			File.Delete (destination);
		}

		for (int i = 1; i <= 100; i++) {
			DeleteFile (i);
		}

		print ("All files deleted");
	}

	private void DeleteFile(int level) {
		string destination = Application.persistentDataPath + "/custom_level" + level + ".dat";
		if (File.Exists (destination)) {
			File.Delete (destination);
		}
	}

}
