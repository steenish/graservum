using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidOnDestroy : MonoBehaviour {

#pragma warning disable
	[SerializeField]
	private GameObject asteroidDeathParticles;
#pragma warning restore

	private bool allowSpawnParticles = true;

	public void DisableSpawning() {
		allowSpawnParticles = false;
	}

	private void OnApplicationQuit() {
		allowSpawnParticles = false;
	}

	private void OnDestroy() {
		if (allowSpawnParticles) {
			GameObject particles = Instantiate(asteroidDeathParticles, transform.position, Quaternion.identity);
			particles.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
		}
	}
}
