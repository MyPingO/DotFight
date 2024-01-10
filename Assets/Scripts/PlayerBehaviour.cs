using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	public void Die()
	{
		Destroy(gameObject);
	}

	private void Respawn(GameObject[] SpawnPoints)
	{
		// TODO: Implement respawn logic
	}
}
