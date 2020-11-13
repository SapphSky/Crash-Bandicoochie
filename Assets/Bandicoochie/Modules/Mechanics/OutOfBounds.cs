using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBounds : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Out of bounds!");
			other.GetComponent<Character>().Kill();
		}
	}
}
