using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

	public string notes;
	public int bpm = 56;
	public Themes theme = Themes.GRASSY;

	public GameData(string notes, int bpm, Themes t)
	{
		this.notes = notes;
		this.bpm = bpm;
		this.theme = t;
	}

}
