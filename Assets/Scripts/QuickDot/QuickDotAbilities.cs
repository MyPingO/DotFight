using System.Collections;
using UnityEngine;

public class QuickDotAbilities : DotAbilitiesBase
{
	private DotMovement movement;
	[SerializeField] private GameObject arrowPrefab;
	new private void Awake() 
	{
		base.Awake();
		movement = GetComponent<DotMovement>();
	}
	protected override void CastMainAbility()
	{
		print("QuickDot main ability");
		SpawnArrow();
	}
	
	protected override void CastSecondaryAbility()
	{
		print("QuickDot secondary ability");
		StartCoroutine(SpeedUp());
	}
	
	private IEnumerator SpeedUp()
	{
		
		float defaultRunSpeed = movement.GetRunSpeed();
		float defaultWalkSpeed = movement.GetWalkSpeed();
		float defaultHorizontalWallJumpForce = movement.GetHorizontalWallJumpForce();
		
		movement.SetRunSpeed(defaultRunSpeed * 2);
		movement.SetWalkSpeed(defaultWalkSpeed * 2);
		movement.SetHorizontalWallJumpForce(defaultHorizontalWallJumpForce * 2);
		movement.UpdateMoveSpeed();
		
		yield return new WaitForSeconds(secondaryAbilityDuration);
		
		movement.SetRunSpeed(defaultRunSpeed);
		movement.SetWalkSpeed(defaultWalkSpeed);
		movement.SetHorizontalWallJumpForce(defaultHorizontalWallJumpForce);
		movement.UpdateMoveSpeed();
	}
	
	private void SpawnArrow() 
	{
		Vector2 direction = GetAimDirection();
		Vector2 spawnPosition = (Vector2) transform.position + direction * 1f;
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
		
		GameObject arrow = Instantiate(arrowPrefab, spawnPosition, rotation);
		arrow.GetComponent<Arrow>().SetCaster(gameObject);
	}
}
