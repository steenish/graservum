using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarter : MonoBehaviour {
    
	public void RestartGame() {
		FindObjectOfType<AsteroidManager>().enabled = false;

		AsteroidOnDestroy[] asteroidParticlesSpawners = FindObjectsOfType<AsteroidOnDestroy>();
		foreach (AsteroidOnDestroy asteroidParticleSpawner in asteroidParticlesSpawners) {
			asteroidParticleSpawner.DisableSpawning();
		}

		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}
}
