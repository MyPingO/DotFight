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
		Vector2 spawnPosition = (Vector2) transform.position + direction * 1f;
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

		Instantiate(fireBallPrefab, spawnPosition, rotation);
	}

	protected override void CastSecondaryAbility()
	{
		StartCoroutine(LeaveFireTrail());
	}
	
	private IEnumerator LeaveFireTrail()
	{
		float elapsedTime = 0f;
		while (elapsedTime < secondaryAbilityDuration)
		{
			if (playerMovement.IsPlayerGrounded() && !isTouchingFireTrail)
			{
				SpawnFireTrail();
			}
			
			elapsedTime += Time.deltaTime;
			yield return null;
		}
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
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("FireTrail"))
		{
			isTouchingFireTrail = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("FireTrail"))
		{
			isTouchingFireTrail = false;
		}
	}
}
