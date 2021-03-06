﻿using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GhostFreeRoamCamera : MonoBehaviour
{
	public float initialSpeed = 10f;
	public float increaseSpeed = 1.25f;

	public bool allowMovement = true;
	public bool allowRotation = true;

	public KeyCode forwardButton = KeyCode.W;
	public KeyCode backwardButton = KeyCode.S;
	public KeyCode rightButton = KeyCode.D;
	public KeyCode leftButton = KeyCode.A;
	public KeyCode shiftLeft = KeyCode.LeftShift;

	public float cursorSensitivity = 0.025f;
	public bool cursorToggleAllowed = true;
	public KeyCode cursorToggleButton = KeyCode.Escape;

	private float currentSpeed = 0f;
	private bool moving = false;
	private bool togglePressed = false;

	private void OnEnable()
	{
		if(cursorToggleAllowed)
		{
			//Screen.lockCursor = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			//Screen.showCursor = false;
		}
	}

	private void Update()
	{
		if(!Input.GetKey(shiftLeft))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			GUI.FocusWindow(1);
			if(allowMovement)
			{
				bool lastMoving = moving;
				Vector3 deltaPosition = Vector3.zero;

				if(moving)
					currentSpeed += increaseSpeed * Time.deltaTime;

				moving = false;

				CheckMove(forwardButton, ref deltaPosition, transform.forward);
				CheckMove(backwardButton, ref deltaPosition, -transform.forward);
				CheckMove(rightButton, ref deltaPosition, transform.right);
				CheckMove(leftButton, ref deltaPosition, -transform.right);

				if(moving)
				{
					if(moving != lastMoving)
						currentSpeed = initialSpeed;

					transform.position += deltaPosition * currentSpeed * Time.deltaTime;
				}
				else currentSpeed = 0f;
			}

			if(allowRotation)
			{
				Vector3 eulerAngles = transform.eulerAngles;
				eulerAngles.x += -Input.GetAxis("Mouse Y") * 359f * cursorSensitivity;
				eulerAngles.y += Input.GetAxis("Mouse X") * 359f * cursorSensitivity;
				if(eulerAngles.x < 89 || eulerAngles.x > 271)
				{
					transform.eulerAngles = eulerAngles;
				}
			}

			if(cursorToggleAllowed)
			{
				if(Input.GetKey(cursorToggleButton))
				{
					if(!togglePressed)
					{
						togglePressed = true;
						if(Cursor.lockState == CursorLockMode.Locked)
						{
							Cursor.lockState = CursorLockMode.Confined;
						}
						else
						{
							Cursor.lockState = CursorLockMode.Locked;
						}
						//Screen.lockCursor = !Screen.lockCursor;
						//Cursor.lockState = !Cursor.lockState;
						Cursor.visible = !Cursor.visible;
						//Screen.showCursor = !Screen.showCursor;
					}
				}
				else togglePressed = false;
			}
			else
			{
				togglePressed = false;
				Cursor.visible = false;
				//Screen.showCursor = false;
			}
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

	}

	private void CheckMove(KeyCode keyCode, ref Vector3 deltaPosition, Vector3 directionVector)
	{
		if(Input.GetKey(keyCode))
		{
			moving = true;
			deltaPosition += directionVector;
		}
	}
}
