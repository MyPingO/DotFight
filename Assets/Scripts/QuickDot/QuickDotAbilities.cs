using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickDotAbilities : PlayerAbilitiesBase
{
	private PlayerMovement playerMovement;
	[SerializeField] private GameObject ArrowPrefab;
	void Awake() 
	{
		playerMovement = GetComponent<PlayerMovement>();
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
		
		float defaultRunSpeed = playerMovement.GetRunSpeed();
		float defaultWalkSpeed = playerMovement.GetWalkSpeed();
		float defaultWallJumpForce = playerMovement.GetWallJumpForce();
		float defaultHorizontalWallJumpForce = playerMovement.GetHorizontalWallJumpForce();
		
		playerMovement.SetRunSpeed(defaultRunSpeed * 2);
		playerMovement.SetWalkSpeed(defaultWalkSpeed * 2);
		playerMovement.SetHorizontalWallJumpForce(defaultHorizontalWallJumpForce * 2);
		playerMovement.UpdateMoveSpeed();
		
		yield return new WaitForSeconds(mainAbilityDuration);
		
		playerMovement.SetRunSpeed(defaultRunSpeed);
		playerMovement.SetWalkSpeed(defaultWalkSpeed);
		playerMovement.SetHorizontalWallJumpForce(defaultHorizontalWallJumpForce);
		playerMovement.UpdateMoveSpeed();
	}
	
	private void SpawnArrow() 
	{
		// rotate arrow to mouse position
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 direction = mousePosition - transform.position;
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
		
		Instantiate(ArrowPrefab, transform.position, rotation);
	}
}
