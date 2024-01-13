using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPoisionAbilities : AIDotAbilitiesBase
{
	// Start is called before the first frame update
	[SerializeField] private GameObject poisonDartPrefab;
	[SerializeField] private GameObject poisonCloudPrefab;
	
	

	protected override void CastMainAbility()
	{
		SpawnPoisonDart();
	}

	protected override void CastSecondaryAbility()
	{
		SpawnPoisonCloud();
	}

	private void SpawnPoisonDart()
	{
		Vector2 direction = GetMouseDirectionNormalized();
		Vector2 spawnPosition = (Vector2) transform.position + direction * 1f;
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

		GameObject poisonDart = Instantiate(poisonDartPrefab, spawnPosition, rotation);
		poisonDart.GetComponent<PoisonDart>().SetCaster(gameObject);
	}

	private void SpawnPoisonCloud()
	{
		// spawn the cloud at the mouse position
		Vector2 mousePosition = GetAIMousePosition();
		GameObject poisonCloud = Instantiate(poisonCloudPrefab, mousePosition, Quaternion.identity);
		poisonCloud.GetComponent<PoisonCloud>().SetDuration(secondaryAbilityDuration);
		poisonCloud.GetComponent<PoisonCloud>().SetCaster(gameObject);
	}
}
