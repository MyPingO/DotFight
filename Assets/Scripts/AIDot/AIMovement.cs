using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIMovement : DotMovement
{
	[SerializeField] private float moveXInput = 0f;

	protected void Start()
	{
		base.Start();
		moveXInput = Random.value > .5f ? 1f : -1f;
		StartCoroutine(CheckPositionStagnation());
	}

	protected override void Update()
	{
		if (IsGrounded())
		{
			moveX = moveXInput; //Snappy movement on the ground and wall
			canAirJump = true;
		}
		else
		{
			// if ai is going left or right in air
			if (moveXInput != 0)
				moveX += moveXInput * Time.deltaTime * airControlSmoothing; // apply smoothed movement in the air
			else
				moveX = 0;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			bool shouldWallJump = Random.value > .7f;
			if (shouldWallJump)
			{
				IsTouchingWall();
				if (isGrounded)
				{
					StartCoroutine(JumpThenWallJump());
				}
				else
				{
					jumpRequested = true;
					StartCoroutine(SwitchDirectionsDelay(.2f));
				}
			}
			else
				moveXInput *= -1f;
		}
	}

	private IEnumerator JumpThenWallJump()
	{
		// assume grounded
		jumpRequested = true; //fixed update will jump
		yield return new WaitForSeconds(.5f);
		jumpRequested = true; // fixed update will wall jump
		yield return new WaitForSeconds(.1f);
		moveXInput *= -1f; //right after wall jump, change direction
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("JumpPoint"))
		{
			JumpPoint jumpPoint = other.gameObject.GetComponent<JumpPoint>();

			if (jumpPoint.randomize != 0 && Random.value > jumpPoint.randomize)
				return;

			if (
				jumpPoint.jumpIfMovingLeft && moveXInput < 0
				|| jumpPoint.jumpIfMovingRight && moveXInput > 0
			)
			{
				jumpRequested = true;
			}
		}
		if (other.gameObject.CompareTag("SwitchPoint"))
		{
			SwitchPoint switchPoint = other.gameObject.GetComponent<SwitchPoint>();

			if (switchPoint.randomize != 0 && Random.value > switchPoint.randomize)
				return;

			moveXInput *= -1f;
		}
	}

	private void RandomizeMovement()
	{
		bool shouldJump = Random.value > .5f;
		bool shouldSwwitchDirection = Random.value > .5f;

		if (shouldJump)
		{
			jumpRequested = true;
		}
		if (shouldSwwitchDirection)
		{
			moveXInput *= -1f;
		}
	}

	private IEnumerator CheckPositionStagnation()
	{
		while (true) // Infinite loop, coroutine will keep running
		{
			float initialX = transform.position.x;
			yield return new WaitForSeconds(0.5f);
			float currentX = transform.position.x;

			if (Mathf.Abs(currentX - initialX) < 0.01f) // Check if the position hasn't changed much
			{
				StartCoroutine(SwitchDirectionsDelay(0)); // Call with no delay
			}
		}
	}

	private IEnumerator SwitchDirectionsDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		moveXInput *= -1f;
	}
}
