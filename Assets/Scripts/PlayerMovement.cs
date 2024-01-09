using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float runSpeed = 5f;
	public float walkSpeed = 2.5f;
	public float jumpForce = 5f;
	public float wallJumpForce = 5f;
	public float wallJumpHorizontalForce = 5f;
	public float inertia = 0.99f;
	public bool jumpRequested = false;
	public bool movementDisabled = false;
	public float airControlSmoothing = 10f;

	[SerializeField]
	private float moveX = 0f;

	[SerializeField]
	private bool isWalking = false;
	
	[SerializeField]
	private bool isJumping = false;

	[SerializeField]
	private bool isWallJumping = false;

	[SerializeField]
	private bool isGrounded = false;

	[SerializeField]
	private bool canAirJump = true; // New variable to save the jump

	[SerializeField]
	private Rigidbody2D rigidBody;

	[SerializeField]
	private Collider2D playerCollider;

	[SerializeField]
	private LayerMask groundLayer;

	[SerializeField]
	private LayerMask wallLayer;

	[SerializeField]
	private RaycastHit2D rightRaycast;

	[SerializeField]
	private RaycastHit2D leftRaycast;

	protected enum PlayerState //TODO: Implement this later if needed
	{
		Idle,
		Walking,
		Jumping,
		WallJumping,
		Falling
	}

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		groundLayer = LayerMask.GetMask("Ground");
		wallLayer = LayerMask.GetMask("Wall");
	}

	private void Update()
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
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			moveSpeed = runSpeed;
		}
	}

	private void FixedUpdate()
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

	private void Jump()
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

	private void WallJump(RaycastHit2D rightRaycast, RaycastHit2D leftRaycast)
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
			wallJumpHorizontalForce * wallJumpDirection,
			wallJumpForce
		);

		isJumping = true;
		isWallJumping = true;
	}

	private IEnumerator DisableMovementAfterWallJump()
	{
		movementDisabled = true;
		yield return new WaitForSeconds(.2f);

		// --*IMPORTANT*-- //
		// set the player's moveX input to be the same as the player's current
		// velocity on the x-axis to prevent resetting the player's momentum
		moveX = rigidBody.velocity.x;
		movementDisabled = false;
	}

	private bool IsGrounded()
	{
		// Check if the bottom of the player touches the ground
		float extraHeight = 0.1f;
		Vector2 raycastStart =
			playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y, 0);
		// Vector2 raycastEnd = raycastStart + Vector2.down * extraHeight;

		// Drawing the line for a duration of 2 seconds with a red color
		// Debug.DrawLine(raycastStart, raycastEnd, Color.red, 2f);

		RaycastHit2D raycastHit = Physics2D.Raycast(
			raycastStart,
			Vector2.down,
			extraHeight,
			groundLayer
		);

		isGrounded = raycastHit.collider != null;

		// // If the ray hits the ground, draw a green line
		// if (isGrounded)
		// {
		//     Debug.DrawLine(raycastStart, raycastEnd, Color.green, 2f);
		// }

		return isGrounded;
	}

	private bool IsTouchingWall()
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
}
