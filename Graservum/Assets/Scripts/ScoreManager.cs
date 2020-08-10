using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager {

	public static int GetHighscore() {
		return PlayerPrefs.HasKey("Highscore") ? PlayerPrefs.GetInt("Highscore") : 0;
	}

	public static void ResetHighscore() {
		PlayerPrefs.SetInt("Highscore", 0);
	}

	public static void SaveHighScore(int score) {
		int currentHighScore = PlayerPrefs.GetInt("Highscore");
		if (score > currentHighScore) {
			PlayerPrefs.SetInt("Highscore", (int) score);
		}
	}
}
