using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float jumpForce = 5f;

	[SerializeField]
	private bool isJumping = false;
	private Rigidbody2D rb;
	private Collider2D playerCollider;
	private LayerMask groundLayer;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		groundLayer = LayerMask.GetMask("Ground");
	}

	private void Update()
	{
		float moveX = Input.GetAxis("Horizontal");

		// Move the player horizontally
		rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

		// Jumping
		if (Input.GetButtonDown("Jump") && !isJumping)
		{
			// Check if the bottom of the player touches the ground
			if (IsGrounded())
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
			}
		}
	}

	private bool IsGrounded()
	{
		// Check if the bottom of the player touches the ground
		float extraHeight = 0.1f;
		Vector2 raycastStart = playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y, 0);
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
}
