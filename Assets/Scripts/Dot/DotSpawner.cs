using UnityEngine;

public class DotSpawner : MonoBehaviour
{
	[SerializeField] private GameObject quickDotPrefab;
	[SerializeField] private GameObject poisonDotPrefab;
	[SerializeField] private GameObject mageDotPrefab;
	[SerializeField] private GameObject pyroDotPrefab;
	[SerializeField] private GameObject AIDotPrefab;
	public GameObject spawnedDot;
	public bool spawnAIDot = false;
	
	void Awake()
	{
		Vector2 spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform.localPosition;

		// Set the playerDot to the dot the player has chosen
		string playerCharacter = PlayerPrefs.GetString("SelectedDot");
		switch (playerCharacter)
		{
			case "FireDot":
				spawnedDot = Instantiate(pyroDotPrefab, spawnPoint, Quaternion.identity);
				break;
			case "MagicDot":
				spawnedDot = Instantiate(mageDotPrefab, spawnPoint, Quaternion.identity);
				break;
			case "PoisonDot":
				spawnedDot = Instantiate(poisonDotPrefab, spawnPoint, Quaternion.identity);
				break;
			case "QuickDot":
				spawnedDot = Instantiate(quickDotPrefab, spawnPoint, Quaternion.identity);
				break;
			default:
				spawnedDot = Instantiate(quickDotPrefab, spawnPoint, Quaternion.identity);
				break;
		}
		Debug.Log($"Instantiated Dot Active State: {spawnedDot.activeSelf}");
		if (spawnAIDot)
		{
			GameObject spawnedAIDot = Instantiate(AIDotPrefab, spawnPoint, Quaternion.identity);
			spawnedAIDot.GetComponent<AIDot>().SetTarget(spawnedDot);
		}
	}

}
