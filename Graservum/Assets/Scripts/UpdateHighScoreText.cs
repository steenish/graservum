using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateHighScoreText : MonoBehaviour {

#pragma warning disable
	[SerializeField]
	private TMP_Text highScoreText;
#pragma warning	restore

	private void OnEnable() {
		highScoreText.text = "Highscore: " + ScoreManager.GetHighscore();
	}
}
