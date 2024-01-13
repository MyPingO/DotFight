using UnityEngine;
using System.Collections;

public class AIQuickAbilities : AIDotAbilitiesBase
{
	[SerializeField] private GameObject ArrowPrefab;
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
		
		float defaultRunSpeed = aiMovement.GetRunSpeed();
		float defaultWalkSpeed = aiMovement.GetWalkSpeed();
		float defaultHorizontalWallJumpForce = aiMovement.GetHorizontalWallJumpForce();
		
		aiMovement.SetRunSpeed(defaultRunSpeed * 2);
		aiMovement.SetWalkSpeed(defaultWalkSpeed * 2);
		aiMovement.SetHorizontalWallJumpForce(defaultHorizontalWallJumpForce * 2);
		aiMovement.UpdateMoveSpeed();
		
		yield return new WaitForSeconds(mainAbilityDuration);
		
		aiMovement.SetRunSpeed(defaultRunSpeed);
		aiMovement.SetWalkSpeed(defaultWalkSpeed);
		aiMovement.SetHorizontalWallJumpForce(defaultHorizontalWallJumpForce);
		aiMovement.UpdateMoveSpeed();
	}
	
	private void SpawnArrow() 
	{
		Vector2 direction = GetMouseDirectionNormalized();
		Vector2 spawnPosition = (Vector2) transform.position + direction * 1f;
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
		
		GameObject arrow = Instantiate(ArrowPrefab, spawnPosition, rotation);
		arrow.GetComponent<Arrow>().SetCaster(gameObject);
	}
}
