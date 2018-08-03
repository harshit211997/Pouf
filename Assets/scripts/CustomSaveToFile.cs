using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class CustomSaveToFile : MonoBehaviour {

	public CarousalView carousal;
	public InputField levelContent;
	public Transform horizontalContainer;
	public GameObject quarter, half, two_eight, whole, rest, measure;

	void Start() {
//		DeleteAll ();
	}

	public void SaveFile()
	{
		string levelCont = levelContent.text;
		levelCont.Trim ();
		string destination = Application.persistentDataPath + "/custom_level" + (GetNoOfCustomLevels () + 1) + ".dat";
		FileStream file;

		if (File.Exists (destination))
			file = File.OpenWrite (destination);
		else
			file = File.Create (destination);

		GameData data = new GameData (levelCont, 56, (Themes)carousal.GetSelectedItemIndex ());
		BinaryFormatter bf = new BinaryFormatter ();
		bf.Serialize (file, data);
		file.Close ();

		SaveLevelInfo (levelCont);

		Debug.Log ("file saved successfully as custom_level" + GetNoOfCustomLevels () + ".dat with " + 56 + " bpm!" + " and with the theme " + (Themes)carousal.GetSelectedItemIndex ());
		ClearAllFields ();
	}

	private void ClearAllFields() {
		int size = horizontalContainer.childCount;
		for (int i = 0; i < size; i++) {
			Destroy (horizontalContainer.GetChild (i).gameObject);
		}

		// Delete the text of level Content
		levelContent.text = "";
	}

	private int GetNoOfCustomLevels() {
		string destination = Application.persistentDataPath + "/custom_levels_info.dat";
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter ();

		if (File.Exists (destination)) {
			file = File.OpenRead (destination);
			LevelsInfo data = (LevelsInfo)bf.Deserialize (file);

			int size = data.levelContent.Count;
			file.Close ();
			return size;
		} else {
			return 0;
		}
	}

	private void SaveLevelInfo(string levelString) {
		string destination = Application.persistentDataPath + "/custom_levels_info.dat";
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter();

		if (File.Exists (destination)) {
			file = File.OpenRead (destination);
			LevelsInfo data = (LevelsInfo) bf.Deserialize(file);

			int size = data.levelContent.Count;
			int level = size + 1;

			data.levelContent.Add (GetContent (levelString));

			file.Close ();
			print ("file exists");

			//Now write the modified content
			file = File.OpenWrite(destination);
			bf.Serialize (file, data);
			file.Close();
		} else {
			file = File.Create (destination);

			List<string> levelsContent = new List<string> ();
			levelsContent.Add(GetContent(levelString));

			print ("file doesn't exist");

			LevelsInfo data = new LevelsInfo (levelsContent);
			bf.Serialize (file, data);
			file.Close();
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

	public void OnClickAddNote(string note) {
		if (horizontalContainer.childCount <= 18) {
			levelContent.text += note + " ";
			//levelContent.Select ();
			if (note.Equals ("q")) {
				GameObject n = Instantiate (quarter);
				n.transform.parent = horizontalContainer;
			} else if (note.Equals ("h")) {
				GameObject n = Instantiate (half);
				n.transform.parent = horizontalContainer;
			} else if (note.Equals ("w")) {
				GameObject n = Instantiate (whole);
				n.transform.parent = horizontalContainer;
			} else if (note.Equals ("r")) {
				GameObject n = Instantiate (rest);
				n.transform.parent = horizontalContainer;
			} else if (note.Equals ("te")) {
				GameObject n = Instantiate (two_eight);
				n.transform.parent = horizontalContainer;
			} else if (note.Equals ("|")) {
				GameObject n = Instantiate (measure);
				n.transform.parent = horizontalContainer;
			}
		}
	}

	public void OnClickDeleteNote() {
		string t = levelContent.text;
		if (t.Length != 0) {
			if (t.Substring (t.Length - 2).Equals ("e ")) {
				t = t.Substring (0, t.Length - 3);
			} else {
				t = t.Substring (0, t.Length - 2);
			}
			levelContent.text = t;

			// Delete the last child of horizontalContainer
			Destroy(horizontalContainer.GetChild(horizontalContainer.childCount - 1).gameObject);
		}
	}
		

	public void OnClickBack() {
		SceneManager.LoadScene ("custom levels");
	}
}
