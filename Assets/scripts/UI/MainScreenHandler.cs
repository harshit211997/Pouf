using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MainScreenHandler : MonoBehaviour {

	public GameObject credits;

	void Start() {
		MakeLevels ();
	}

	private void MakeLevels() {
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter();
		string levelString;
		GameData data;

		// Insert necessary info in the levels files and levels_info files

		// Level 1
		string destination = Application.persistentDataPath + "/level1.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "q q q q | q q q q | q q q q | q q q q |q q q q | q q q q | q q q q | q q q q";
			data = new GameData (levelString,56,Themes.GRASSY);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (1, levelString);
			print ("level 1 created");
		}

		// Level 2
		destination = Application.persistentDataPath + "/level2.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "q r q r | q r q r | q r q r | q r q r | q r q r | q r q r | q r q r | q r q r";
			data = new GameData (levelString, 56, Themes.DUSK);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (2, levelString);
			print ("level 2 created");
		}

		// Level 3
		destination = Application.persistentDataPath + "/level3.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "h h | h h | h h | h h | h h | h h | h h | h h";
			data = new GameData (levelString, 56, Themes.DUSK);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (3, levelString);
			print ("level 3 created");
		}

		// Level 4
		destination = Application.persistentDataPath + "/level4.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "h r r | h r r | h r r | h r r | h r r | h r r | h r r | h r r";
			data = new GameData (levelString, 56, Themes.NIGHT);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (4, levelString);
			print ("level 4 created");
		}

		// Level 5
		destination = Application.persistentDataPath + "/level5.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "te te te te | te te te te | te te te te | te te te te | te te te te | te te te te | te te te te | te te te te";
			data = new GameData (levelString, 56, Themes.NIGHT);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (5, levelString);
			print ("level 5 created");
		}

		// Level 6
		destination = Application.persistentDataPath + "/level6.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "te r te r | te r te r | te r te r | te r te r | te r te r | te r te r | te r te r | te r te r";
			data = new GameData (levelString, 56, Themes.GRASSY);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (6, levelString);
			print ("level 6 created");
		}

		// Level 7
		destination = Application.persistentDataPath + "/level7.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "w | w | w | w | w | w | w | w";
			data = new GameData (levelString, 56, Themes.GOTHIC);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (7, levelString);
			print ("level 7 created");
		}

		// Level 8
		destination = Application.persistentDataPath + "/level8.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "q te q te | q te q te | q te q te | q te q te | q te q te | q te q te | q te q te | q te q te";
			data = new GameData (levelString, 56, Themes.GOTHIC);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (8, levelString);
			print ("level 8 created");
		}

		// Level 9
		destination = Application.persistentDataPath + "/level9.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "te h te | te h te | te h te | te h te | te h te | te h te | te h te | te h te";
			data = new GameData (levelString, 56, Themes.VOLCANO);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (9, levelString);
			print ("level 9 created");
		}

		// Level 10
		destination = Application.persistentDataPath + "/level10.dat";
		if (!File.Exists (destination)) {
			file = File.Create (destination);
			levelString = "q q q q | te te te te | te r q te | r h r | q q q q | te te te te | te r q te | r h r";
			data = new GameData (levelString, 56, Themes.GOTHIC);
			bf.Serialize (file, data);
			file.Close ();
			SaveLevelInfo (10, levelString);
			print ("level 10 created");
		}
	}

	private void SaveLevelInfo(int level, string levelString) {
		string destination = Application.persistentDataPath + "/levels_info.dat";
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter();

		if (File.Exists (destination)) {
			file = File.OpenRead (destination);
			LevelsInfo data = (LevelsInfo) bf.Deserialize(file);

			int size = data.levelContent.Count;
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

	public void OnClickPlay() {
		Application.targetFrameRate = 60;
		if (PlayerPrefs.GetInt ("tutorial", -1) == -1) {
			// Open tutorial screen
			LoadingScreenManager.LoadScene (7);
			PlayerPrefs.SetInt ("tutorial", 1);
		} else {
			LoadingScreenManager.LoadScene (1);
		}
	}

	public void OnClickBack() {
		credits.SetActive (false);
	}

	public void OnClickInfo() {
		credits.SetActive (true);
	}
}
