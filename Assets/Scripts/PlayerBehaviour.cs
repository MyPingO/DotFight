using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	[SerializeField] private float timeInPoison;
	[SerializeField] private float timeUntilPoisonFatal;
	public void BecomePoisoned(float poisonTime)
	{
		Die(poisonTime);
	}
	public void Die(float timeDelay = 0f)
	{
		Destroy(gameObject, timeDelay);
	}

	private void Respawn(GameObject[] SpawnPoints)
	{
		// TODO: Implement respawn logic
		
	}
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Poison"))
		{
			timeInPoison += Time.deltaTime;
			if (timeInPoison >= timeUntilPoisonFatal)
			{
				Die();
			}
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Poison"))
		{
			timeInPoison = 0f;
		}
	}
}
