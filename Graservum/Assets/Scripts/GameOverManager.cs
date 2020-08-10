using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

#pragma warning disable
	[SerializeField]
	private TMP_Text gameScoreText;
	[SerializeField]
	private TMP_Text scoreText;
	[SerializeField]
	private TMP_Text highScoreText;
	[SerializeField]
	private PlayerInput playerInput;
#pragma warning restore

	private void OnEnable() {
		gameScoreText.gameObject.SetActive(false);

		ScoreManager.SaveHighScore((int) playerInput.score);

		scoreText.text = "Score: " + (int) playerInput.score;
		highScoreText.text = "Highscore: " + ScoreManager.GetHighscore();
	}

	public void RestartGame() {
		DisableAstroidDeathParticles();
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}

	public void ReturnToMenu() {
		DisableAstroidDeathParticles();
		SceneManager.LoadSceneAsync("MainMenuScene");
	}

	private void DisableAstroidDeathParticles() {
		FindObjectOfType<AsteroidManager>().enabled = false;

		AsteroidOnDestroy[] asteroidParticlesSpawners = FindObjectsOfType<AsteroidOnDestroy>();
		foreach (AsteroidOnDestroy asteroidParticleSpawner in asteroidParticlesSpawners) {
			asteroidParticleSpawner.DisableSpawning();
		}
	}
}
