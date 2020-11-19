using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour {
	public CharacterController controller;

	[Header("Settings")]
	public bool enableControl = true;

	[Header("Collision Check")]
	public LayerMask groundMask;

	[Header("Locomotion")]
	public float moveSpeed = 5.0f;
	public float jumpHeight = 2.0f;
	public int maxMidairJumps = 2;
	public int midairJumps = 2;

	[Header("Runtime Parameters")]
	public Vector3 velocity = new Vector3(0, 0, 0);
	public bool isGrounded = false;

	public UnityEvent onKill;
	public UnityEvent onJump, onMidairJump;

	private void Start() {
		if (controller == null) {
			controller = GetComponent<CharacterController>();
		}
	}

	private void Update() {
		isGrounded = Physics.CheckSphere(transform.position, controller.skinWidth * 1.10f, groundMask, QueryTriggerInteraction.Ignore);
		if (isGrounded) {
			if (velocity.y < 0) {
				velocity.y = Physics.gravity.y * Time.deltaTime;

				if (midairJumps != maxMidairJumps) {
					midairJumps = maxMidairJumps;
				}
			}
		}

		velocity += Physics.gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	public void Move(Vector3 direction) {
		var moveVector = direction * moveSpeed * Time.deltaTime;
		controller.Move(direction * moveSpeed * Time.deltaTime);
	}

	public void Jump() {
		if (isGrounded) {
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
			onJump.Invoke();
		}
		else if (midairJumps > 0) {
			midairJumps--;
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
			onMidairJump.Invoke();
		}
	}

	public void Kill() {
		StartCoroutine(PlayDeathAnimation());
		onKill.Invoke();
	}

	public IEnumerator PlayDeathAnimation() {
		controller.enabled = false;
		GetComponent<Rigidbody>().isKinematic = false;

		yield return new WaitForSeconds(3.0f);
		
		Debug.Log($"Checkpoint set to {Checkpoint.activeCheckpoint.transform.position}");
		gameObject.transform.position = Checkpoint.activeCheckpoint.transform.position;

		controller.enabled = true;
		GetComponent<Rigidbody>().isKinematic = true;
	}
}
