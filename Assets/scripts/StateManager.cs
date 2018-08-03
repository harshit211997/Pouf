using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager {

	private static int level = 0;
	private static Themes theme = Themes.GRASSY;
	private static bool isCustom = false;

	public static void SetLevel(int level) {
		StateManager.level = level;
	}

	public static int GetLevel() {
		return level;
	}

	public static void SetTheme(Themes theme) {
		StateManager.theme = theme;
	}

	public static int GetTheme() {
		return (int)theme;
	}

	public static bool IsCustom() {
		return isCustom;
	}

	public static void SetIsCustom(bool isCustom) {
		StateManager.isCustom = isCustom;
	}

}
