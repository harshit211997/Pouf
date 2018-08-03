using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelsInfo {

	public List<string> levelContent;

	public LevelsInfo(List<string> levelContent) {
		this.levelContent = levelContent;
	}
}
