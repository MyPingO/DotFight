using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float jumpForce = 5f;
	public float wallJumpForce = 5f;
	public float wallJumpHorizontalForce = 5f;

	[SerializeField]
	private bool isJumping = false;

	[SerializeField]
	private bool isWallJumping = false;
	private Rigidbody2D rb;
	private Collider2D playerCollider;
	private LayerMask groundLayer;
	private LayerMask wallLayer;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		groundLayer = LayerMask.GetMask("Ground");
		wallLayer = LayerMask.GetMask("Wall");
	}

	private void Update()
	{
		float moveX = Input.GetAxis("Horizontal");

		// Move the player horizontally
		rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

		// Jumping
		if (Input.GetButtonDown("Jump"))
		{
			if (IsGrounded())
			{
				Jump();
			}
			else if (IsTouchingWall())
			{
				WallJump();
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Reset jump state when touching the ground
		if (collision.gameObject.CompareTag("Ground"))
		{
			if (IsGrounded())
			{
				isJumping = false;
				isWallJumping = false;
			}
		}
	}

	private void Jump()
	{
		// Calculate additional jump force based on the player's velocity on the x-axis
		float additionalJumpForce = Mathf.Abs(rb.velocity.x) * 0.5f;
		if (rb.velocity.y != 0f)
		{
			rb.velocity = new Vector2(rb.velocity.x, 0);
		}
		// Apply the jump force with additional force
		rb.AddForce(new Vector2(0f, jumpForce + additionalJumpForce), ForceMode2D.Impulse);
		isJumping = true;
	}

	private void WallJump()
	{
		// Calculate wall jump direction
		float wallJumpDirection = Input.GetAxis("Horizontal") < 0 ? 1f : -1f;

		// Apply wall jump force
		rb.velocity = new Vector2(wallJumpHorizontalForce * wallJumpDirection, jumpForce);
		isJumping = true;
		isWallJumping = true;
	}

	private bool IsGrounded()
	{
		// Check if the bottom of the player touches the ground
		float extraHeight = 0.1f;
		Vector2 raycastStart =
			playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y, 0);
		Vector2 raycastEnd =
			raycastStart + Vector2.down * (playerCollider.bounds.extents.y + extraHeight);

		// Drawing the line for a duration of 2 seconds with a red color
		Debug.DrawLine(raycastStart, raycastEnd, Color.red, 2f);

		RaycastHit2D raycastHit = Physics2D.Raycast(
			raycastStart,
			Vector2.down,
			playerCollider.bounds.extents.y + extraHeight,
			groundLayer
		);
		bool isGrounded = raycastHit.collider != null;

		// If the ray hits the ground, draw a green line
		if (isGrounded)
		{
			Debug.DrawLine(raycastStart, raycastEnd, Color.green, 2f);
		}

		return isGrounded;
	}

	private bool IsTouchingWall()
	{
		// Check if the player is touching a wall
		float extraWidth = 0.01f;
		
		// Start at right side of the player
		Vector2 raycastStartRight =
			playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0, 0);
		Vector2 raycastEndRight = raycastStartRight + Vector2.right * extraWidth;

		// Start at left side of the player
		Vector2 raycastStartLeft =
			playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0, 0);
		Vector2 raycastEndLeft = raycastStartLeft - Vector2.right * extraWidth;

		// Drawing the lines for a duration of 2 seconds with a red color assuming it doesn't hit anything
		Debug.DrawLine(raycastStartRight, raycastEndRight, Color.red, 2f);
		Debug.DrawLine(raycastStartLeft, raycastEndLeft, Color.red, 2f);

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
		if (raycastHitRight.collider != null)
		{
			Debug.Log("Right raycast hit: " + raycastHitRight.collider.gameObject.name);
			Debug.DrawLine(raycastStartRight, raycastEndRight, Color.green, 2f);
		}

		if (raycastHitLeft.collider != null)
		{
			Debug.Log("Left raycast hit: " + raycastHitLeft.collider.gameObject.name);
			Debug.DrawLine(raycastStartLeft, raycastEndLeft, Color.green, 2f);
		}
		
		bool isTouchingWall = raycastHitRight.collider != null || raycastHitLeft.collider != null;
		return isTouchingWall;
	}
}
