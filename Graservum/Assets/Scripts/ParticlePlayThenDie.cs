using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticlePlayThenDie : MonoBehaviour {

#pragma warning disable
	[SerializeField]
	private ParticleSystem particles;
#pragma warning restore

	private void Start() {
		StartCoroutine(PlayThenDie());
    }

	private IEnumerator PlayThenDie() {
		yield return new WaitForSeconds(particles.main.duration);
		Destroy(gameObject);
	}
}
