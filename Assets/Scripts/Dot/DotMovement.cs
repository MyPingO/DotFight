using System.Collections;
using UnityEngine;
using FishNet.Object;

public class DotMovement : NetworkBehaviour
{
	[SerializeField]
	protected float moveSpeed = 5f;

	[SerializeField]
	protected float runSpeed = 5f;

	[SerializeField]
	protected float walkSpeed = 2.5f;

	[SerializeField]
	protected float jumpForce = 5f;

	[SerializeField]
	protected float wallJumpForce = 5f;

	[SerializeField]
	protected float horizontalWallJumpForce = 5f;
	public float inertia = 0.99f;
	public bool jumpRequested = false;
	public bool movementDisabled = false;
	public float airControlSmoothing = 10f;

	[SerializeField]
	protected float moveX = 0f;

	[SerializeField]
	protected bool isWalking = false;

	[SerializeField]
	protected bool isJumping = false;

	[SerializeField]
	protected bool isWallJumping = false;

	[SerializeField]
	protected bool isGrounded = false;

	[SerializeField]
	protected bool canAirJump = true; // New variable to save the jump

	[SerializeField]
	protected Rigidbody2D rigidBody;

	[SerializeField]
	protected Collider2D playerCollider;

	[SerializeField]
	protected LayerMask groundLayer;

	[SerializeField]
	protected LayerMask wallLayer;

	[SerializeField]
	protected RaycastHit2D rightRaycast;

	[SerializeField]
	protected RaycastHit2D leftRaycast;
	
	[SerializeField]
	protected RaycastHit2D groundRaycast;

	protected enum PlayerState //TODO: Implement this later if needed
	{
		Idle,
		Walking,
		Jumping,
		WallJumping,
		Falling
	}

	protected void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		groundLayer = LayerMask.GetMask("Ground");
		wallLayer = LayerMask.GetMask("Wall");
	}

	protected virtual void Update()
	{
		float moveXInput = Input.GetAxisRaw("Horizontal");
		if (IsGrounded())
		{
			moveX = moveXInput; //Snappy movement on the ground and wall
			canAirJump = true;
		}
		else
		{
			// if player is holding left or right in air
			if (moveXInput != 0)
				moveX += moveXInput * Time.deltaTime * airControlSmoothing; // apply smoothed movement in the air
			else
				moveX = 0;
		}

		if (Input.GetButtonDown("Jump"))
		{
			jumpRequested = true;
		}
		// If Left Shift
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			moveSpeed = walkSpeed;
			isWalking = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			moveSpeed = runSpeed;
			isWalking = false;
		}
	}

	protected virtual void FixedUpdate()
	{
		if (movementDisabled)
			return;

		moveX = Mathf.Clamp(moveX, -1f, 1f);
		// Move the player horizontally
		if (moveX != 0f)
			rigidBody.velocity = new Vector2(moveX * moveSpeed, rigidBody.velocity.y);
		// If the player is not moving horizontally, slow down the player gradually to a stop
		else
			rigidBody.velocity = new Vector2(rigidBody.velocity.x * inertia, rigidBody.velocity.y);
		
		// Jumping
		if (jumpRequested)
		{
			if (isGrounded)
			{
				Jump();
				canAirJump = false;
			}
			else if (IsTouchingWall())
			{
				WallJump(rightRaycast, leftRaycast);
				StartCoroutine(DisableMovementAfterWallJump());
			}
			else if (canAirJump)
			{
				Jump();
				canAirJump = false;
			}
			jumpRequested = false;
		}
	}

	protected void Jump()
	{
		// Calculate additional jump force based on the player's velocity on the x-axis
		float additionalJumpForce = Mathf.Abs(rigidBody.velocity.x) * 0.5f;
		if (rigidBody.velocity.y != 0f)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
		}

		// Set jump velocity
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce + additionalJumpForce);

		isJumping = true;
	}

	protected void WallJump(RaycastHit2D rightRaycast, RaycastHit2D leftRaycast)
	{
		// Calculate wall jump direction
		float wallJumpDirection = 0;

		if (rightRaycast.collider != null)
		{
			wallJumpDirection = -1f;
		}
		else if (leftRaycast.collider != null)
		{
			wallJumpDirection = 1f;
		}

		// Reset velocity
		rigidBody.velocity = new Vector2(0f, 0f);
		moveX = 0;

		// Set walljump velocity
		rigidBody.velocity = new Vector2(
			horizontalWallJumpForce * wallJumpDirection,
			wallJumpForce
		);

		isJumping = true;
		isWallJumping = true;
	}

	protected IEnumerator DisableMovementAfterWallJump()
	{
		movementDisabled = true;
		yield return new WaitForSeconds(.2f);

		// --*IMPORTANT*-- //
		// set the player's moveX input to be the same as the player's current
		// velocity on the x-axis to prevent resetting the player's momentum
		moveX = rigidBody.velocity.x;
		movementDisabled = false;
	}

	public bool IsGrounded()
	{
		// Check if the bottom of the player touches the ground
		float extraHeight = 0.1f;
		Vector2 raycastStart =
			playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y, 0);

		// Downward Raycast
		Vector2 downRay = raycastStart + Vector2.down * extraHeight;
		RaycastHit2D hitDown = Physics2D.Raycast(
			raycastStart,
			Vector2.down,
			extraHeight,
			groundLayer
		);
		// Debug.DrawLine(raycastStart, downRay, hitDown ? Color.green : Color.red, 2f);

		// Down-Right Raycast
		Vector2 downRightRay =
			raycastStart + (Vector2.down + Vector2.right).normalized * extraHeight;
		RaycastHit2D hitDownRight = Physics2D.Raycast(
			raycastStart,
			(Vector2.down + Vector2.right).normalized,
			extraHeight,
			groundLayer
		);
		// Debug.DrawLine(raycastStart, downRightRay, hitDownRight ? Color.green : Color.red, 2f);

		// Down-Left Raycast
		Vector2 downLeftRay = raycastStart + (Vector2.down + Vector2.left).normalized * extraHeight;
		RaycastHit2D hitDownLeft = Physics2D.Raycast(
			raycastStart,
			(Vector2.down + Vector2.left).normalized,
			extraHeight,
			groundLayer
		);
		// Debug.DrawLine(raycastStart, downLeftRay, hitDownLeft ? Color.green : Color.red, 2f);

		// Checking if any of the rays hit the ground
		isGrounded =
			hitDown.collider != null
			|| hitDownRight.collider != null
			|| hitDownLeft.collider != null;
			
		if (isGrounded)
		{
			if (hitDown.collider != null)
			{
				groundRaycast = hitDown;
			}
			else if (hitDownRight.collider != null)
			{
				groundRaycast = hitDownRight;
			}
			else if (hitDownLeft.collider != null)
			{
				groundRaycast = hitDownLeft;
			}
		}

		return isGrounded;
	}

	protected bool IsTouchingWall()
	{
		// Check if the player is touching a wall
		float extraWidth = 0.01f;

		// Start at right side of the player
		Vector2 raycastStartRight =
			playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0, 0);
		// Vector2 raycastEndRight = raycastStartRight + Vector2.right * extraWidth;

		// Start at left side of the player
		Vector2 raycastStartLeft =
			playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0, 0);
		// Vector2 raycastEndLeft = raycastStartLeft - Vector2.right * extraWidth;

		// Drawing the lines for a duration of 2 seconds with a red color assuming it doesn't hit anything
		// Debug.DrawLine(raycastStartRight, raycastEndRight, Color.red, 2f);
		// Debug.DrawLine(raycastStartLeft, raycastEndLeft, Color.red, 2f);

		// Throw a raycast to the right and left of the player
		RaycastHit2D raycastHitRight = Physics2D.Raycast(
			raycastStartRight,
			Vector2.right,
			extraWidth,
			wallLayer
		);
		RaycastHit2D raycastHitLeft = Physics2D.Raycast(
			raycastStartLeft,
			-Vector2.right,
			extraWidth,
			wallLayer
		);

		// If the ray hits the wall, draw a green line for the corresponding ray
		// if (raycastHitRight.collider != null)
		// {
		// 	Debug.Log("Right raycast hit: " + raycastHitRight.collider.gameObject.name);
		// 	Debug.DrawLine(raycastStartRight, raycastEndRight, Color.green, 2f);
		// }

		// if (raycastHitLeft.collider != null)
		// {
		// 	Debug.Log("Left raycast hit: " + raycastHitLeft.collider.gameObject.name);
		// 	Debug.DrawLine(raycastStartLeft, raycastEndLeft, Color.green, 2f);
		// }

		rightRaycast = raycastHitRight;
		leftRaycast = raycastHitLeft;

		bool isTouchingWall = raycastHitRight.collider != null || raycastHitLeft.collider != null;
		return isTouchingWall;
	}

	public float GetRunSpeed()
	{
		return runSpeed;
	}

	public void SetRunSpeed(float newRunSpeed)
	{
		runSpeed = newRunSpeed;
	}

	public float GetWalkSpeed()
	{
		return walkSpeed;
	}

	public void SetWalkSpeed(float newWalkSpeed)
	{
		walkSpeed = newWalkSpeed;
	}

	public float GetJumpForce()
	{
		return jumpForce;
	}

	public void SetJumpForce(float newJumpForce)
	{
		jumpForce = newJumpForce;
	}

	public float GetWallJumpForce()
	{
		return wallJumpForce;
	}

	public void SetWallJumpForce(float newWallJumpForce)
	{
		wallJumpForce = newWallJumpForce;
	}

	public float GetHorizontalWallJumpForce()
	{
		return horizontalWallJumpForce;
	}

	public void SetHorizontalWallJumpForce(float newWallJumpHorizontalForce)
	{
		horizontalWallJumpForce = newWallJumpHorizontalForce;
	}
	
	public float GetMoveX()
	{
		return moveX;
	}

	public void UpdateMoveSpeed()
	{
		moveSpeed = isWalking ? walkSpeed : runSpeed;
	}
	
	public bool IsPlayerGrounded()
	{
		return isGrounded;
	}
	
	public RaycastHit2D GetGroundRaycast()
	{
		return groundRaycast;
	}
}
