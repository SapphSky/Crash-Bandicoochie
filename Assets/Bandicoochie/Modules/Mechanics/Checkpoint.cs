using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	public static Checkpoint activeCheckpoint;
	public bool active;

	public void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") && active) {
			activeCheckpoint = this;
			active = false;
		}
	}
}
