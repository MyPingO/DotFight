using System.Collections;
using UnityEngine;

public class FireDotAbilities : DotAbilitiesBase
{
	[SerializeField] private GameObject fireBallPrefab;
	[SerializeField] private GameObject fireTrailPrefab;
	[SerializeField] private DotMovement playerMovement;
	[SerializeField] private bool isTouchingFireTrail;
	[SerializeField] private float fireTrailSpawnDistance;
	[SerializeField] private Vector2 lastFireTrailSpawnPosition;
	
	new private void Awake()
	{
		base.Awake();
		playerMovement = GetComponent<DotMovement>();
		fireTrailSpawnDistance = fireTrailPrefab.GetComponent<BoxCollider2D>().size.x / 2;
	}
	
	protected override void CastMainAbility()
	{
		SpawnFireBall();
	}
	
	private void SpawnFireBall()
	{
		Vector2 direction = GetAimDirection();
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
		while (elapsedTime < secondaryAbilityDuration)
		{
			if(Vector3.Distance(transform.position, lastFireTrailSpawnPosition) >= fireTrailSpawnDistance && !IsTouchingFireTrail())
			{
				SpawnFireTrail();
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	private bool IsTouchingFireTrail()
	{
		LayerMask fireTrailLayerMask = LayerMask.GetMask("FireTrail");
		float radius = GetComponent<Collider2D>().bounds.size.x;
		
		// use overlapcircle
		Collider2D fireTrailCollider = Physics2D.OverlapCircle(transform.position, radius, fireTrailLayerMask);
		return fireTrailCollider != null;
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
		lastFireTrailSpawnPosition = groundRaycastHit.point;
	}
}
