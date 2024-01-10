using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicDotAbilities : PlayerAbilitiesBase
{
	[SerializeField] private GameObject MagicBallPrefab;
	[SerializeField] private GameObject BarrierPrefab;
	[SerializeField] private Transform target;
	protected override void CastMainAbility()
	{
		print("MagicDot main ability");
		SpawnMagicBall();
	}
	
	private void SpawnMagicBall() 
	{
		GameObject magicBallGO = Instantiate(MagicBallPrefab, transform.position, Quaternion.identity);
		MagicBall magicBall = magicBallGO.GetComponent<MagicBall>();
		magicBall.SetTarget(target);
		magicBall.SetCaster(gameObject);
		magicBall.LifeTime(mainAbilityDuration);
	}
	private void GetClosestTarget(out Transform target)
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		GameObject closestPlayer = null;
		float closestDistance = Mathf.Infinity;

		foreach (GameObject player in players)
		{
			if (player != gameObject) // Exclude the current player who is casting the spell
			{
				float distance = Vector3.Distance(transform.position, player.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestPlayer = player;
				}
			}
		}

		if (closestPlayer) target = closestPlayer.transform;
		else target = null;
	}

	protected override void CastSecondaryAbility()
	{
		print("MagicDot secondary ability");
		SpawnBarrier();
	}
	
	private void SpawnBarrier() 
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0f; // Set z to 0 so that the barrier is on the same plane as the player
		float mouseHorizontalDistanceFromPlayer = Mathf.Abs(mousePosition.x - transform.position.x);
		
		GameObject magicBarrier;
		GameObject flatSide;
		magicBarrier = Instantiate(BarrierPrefab, mousePosition, Quaternion.identity);
		flatSide = magicBarrier.transform.Find("FlatSide").gameObject;

		
		//Check if mouse click is close to under the player 
		if (mousePosition.y < transform.position.y && mouseHorizontalDistanceFromPlayer <= 5)
		{
			magicBarrier.transform.eulerAngles = new Vector3(0, 0, 0);
			flatSide.layer =  LayerMask.NameToLayer("Ground");
		}
		// Check if mouse click is close to above the player
		else if (mousePosition.y > transform.position.y && mouseHorizontalDistanceFromPlayer <= 1.5f)
		{
			magicBarrier.transform.eulerAngles = new Vector3(0, 0, 180);
			flatSide.layer =  LayerMask.NameToLayer("Ground");
		}
		// If mouse click is to the sides of the player
		else
		{
			if (mousePosition.x > transform.position.x) magicBarrier.transform.eulerAngles = new Vector3(0, 0, 90);
			else magicBarrier.transform.eulerAngles = new Vector3(0, 0, -90);
			
			flatSide.layer =  LayerMask.NameToLayer("Wall");
		}
		
		Destroy(magicBarrier, secondaryAbilityDuration);
	}

	protected override void Update()
	{
		currentMainAbilityCooldown = Mathf.Max(0, currentMainAbilityCooldown - Time.deltaTime);
		currentSecondaryAbilityCooldown = Mathf.Max(0, currentSecondaryAbilityCooldown - Time.deltaTime);
		
		if (Input.GetMouseButtonDown(0) && currentMainAbilityCooldown <= 0)
		{
			// Only cast main ability if there is a target
			GetClosestTarget(out target);
			if (target != null)
			{
				CastMainAbility();
				currentMainAbilityCooldown = mainAbilityCooldown;
			}
		}

		if (Input.GetMouseButtonDown(1) && currentSecondaryAbilityCooldown <= 0)
		{
			CastSecondaryAbility();
			currentSecondaryAbilityCooldown = secondaryAbilityCooldown;
		}
	}
}
