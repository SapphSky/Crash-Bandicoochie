using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour {
	public Character character;

	[Header("Input Settings")]
	public string horizontalMoveInput = "Horizontal";
	public string verticalMoveInput = "Vertical";
	public string jumpInput = "Jump";

	[Header("Runtime Parameters")]
	[SerializeField] public float turnSmoothTime = 0.1f;
	[SerializeField] private float turnSmoothVelocity;
	[SerializeField] private Transform cam;

	private void Start() {
		character = GetComponent<Character>();
	}

	private void Update() {
		Vector3 moveInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxisRaw(horizontalMoveInput), 0, Input.GetAxisRaw(verticalMoveInput)), 1);
		Vector3 moveDir = Vector3.zero;

		if (moveInput.magnitude >= 0.01f) {
			float targetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			character.Move(moveDir.normalized);
		}

		if (Input.GetButtonDown(jumpInput)) {
			character.Jump();
		}
	}
}
