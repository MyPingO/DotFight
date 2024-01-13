using System.Collections;
using UnityEngine;

public class FireDotAbilities : PlayerAbilitiesBase
{
	[SerializeField] private GameObject fireBallPrefab;
	[SerializeField] private GameObject fireTrailPrefab;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private bool isTouchingFireTrail;
	
	private void Awake()
	{
		playerMovement = GetComponent<PlayerMovement>();
	}
	
	protected override void CastMainAbility()
	{
		SpawnFireBall();
	}
	
	private void SpawnFireBall()
	{
		Vector2 direction = GetMouseDirectionNormalized();
		Vector2 spawnPosition = (Vector2) transform.position;
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

		GameObject fireball = Instantiate(fireBallPrefab, spawnPosition, rotation);
		fireball.GetComponent<FireBall>().SetCaster(gameObject);
	}
	protected override void CastSecondaryAbility()
	{
		StartCoroutine(LeaveFireTrail());
	}
	
	private IEnumerator LeaveFireTrail()
	{
		float elapsedTime = 0f;
		float fireTrailSpawnInterval = 0.1f;
		while (elapsedTime < secondaryAbilityDuration)
		{
			if (playerMovement.IsPlayerGrounded() && !IsTouchingFireTrail())
			{
				SpawnFireTrail();
				fireTrailSpawnInterval = 0.1f;
			}
			fireTrailSpawnInterval -= Time.deltaTime;
			
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	private bool IsTouchingFireTrail()
	{
		float raycastDistance = 0.05f;
		LayerMask fireTrailLayerMask = LayerMask.GetMask("FireTrail");
		
		Collider2D playerCollider = GetComponent<Collider2D>();
		Vector2 raycastStartLeft = playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0, 0);
		Vector2 raycastStartRight = playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0, 0);
		
		RaycastHit2D leftRaycastHit = Physics2D.Raycast(raycastStartLeft, Vector2.left, raycastDistance, fireTrailLayerMask);
		RaycastHit2D rightRaycastHit = Physics2D.Raycast(raycastStartRight, Vector2.right, raycastDistance, fireTrailLayerMask);
		
		// // Draw raycasts
		// Debug.DrawRay(raycastStartLeft, Vector2.left * raycastDistance, leftRaycastHit.collider != null ? Color.green : Color.red);
		// Debug.DrawRay(raycastStartRight, Vector2.right * raycastDistance, rightRaycastHit.collider != null ? Color.green : Color.red);
		
		// // print what the raycasts hit
		// if (leftRaycastHit.collider != null)
		// {
		// 	print("Left raycast hit: " + leftRaycastHit.collider.gameObject.name);
		// }
		// if (rightRaycastHit.collider != null)
		// {
		// 	print("Right raycast hit: " + rightRaycastHit.collider.gameObject.name);
		// }
		
		return leftRaycastHit.collider != null || rightRaycastHit.collider != null;
	}
	
	private void SpawnFireTrail()
	{
		RaycastHit2D groundRaycastHit = playerMovement.GetGroundRaycast();
		if (groundRaycastHit)
		{
			GameObject fireTrailGO = Instantiate(fireTrailPrefab, groundRaycastHit.point, Quaternion.LookRotation(Vector3.forward, groundRaycastHit.normal));
			FireTrail fireTrail = fireTrailGO.GetComponent<FireTrail>();
			fireTrail.SetCaster(gameObject);
		}
	}
}
