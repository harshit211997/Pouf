using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveToFile : MonoBehaviour {

	public CarousalView carousal;
	public InputField levelInput, levelContent;
	public Slider bpmSlider;

	public void SaveFile()
	{
		string destination = Application.persistentDataPath + "/level" + levelInput.text + ".dat";
		FileStream file;

		if(File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		GameData data = new GameData(levelContent.text, (int)bpmSlider.value, (Themes)carousal.GetSelectedItemIndex());
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();

		SaveLevelInfo (levelContent.text);

		Debug.Log ("file saved successfully as level" + levelInput.text + ".dat with " + bpmSlider.value + " bpm!" + " and with the theme " + (Themes)carousal.GetSelectedItemIndex());
	}

	private void SaveLevelInfo(string levelString) {
		string destination = Application.persistentDataPath + "/levels_info.dat";
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter();

		if (File.Exists (destination)) {
			file = File.OpenRead (destination);
			LevelsInfo data = (LevelsInfo) bf.Deserialize(file);

			int size = data.levelContent.Count;
			int level = int.Parse (levelInput.text);
			if (level <= size) {
				int lockStatus = data.levelContent [level - 1].IndexOf ("l");
				data.levelContent [level - 1] = GetContent (levelString);
				if (lockStatus != -1) {
					data.levelContent [level - 1] = "l " + data.levelContent [level - 1];
				} 
			} else {
				data.levelContent.Add (GetContent (levelString));
			}
			file.Close ();

			//Now write the modified content
			file = File.OpenWrite(destination);
			bf.Serialize (file, data);
		} else {
			file = File.Create (destination);

			List<string> levelsContent = new List<string> ();
			levelsContent.Add(GetContent(levelString));

			LevelsInfo data = new LevelsInfo (levelsContent);
			bf.Serialize (file, data);
			return;
		}
		file.Close();
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

	public void Reset() {
		PlayerPrefs.SetInt ("locked from", -1);
	}

	public void DeleteAll() {
		// Delete levels info file
		string destination = Application.persistentDataPath + "/levels_info.dat";
		if (File.Exists (destination)) {
			File.Delete (destination);
		}

		for (int i = 1; i <= 10; i++) {
			DeleteFile (i);
		}

		print ("All files deleted");
	}

	private void DeleteFile(int level) {

		string destination = Application.persistentDataPath + "/level" + level + ".dat";
		if (File.Exists (destination)) {
			File.Delete (destination);
		}
	}
}
