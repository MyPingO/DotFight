using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	[SerializeField]
	private float timeInPoison;

	[SerializeField]
	private float timeUntilPoisonFatal;

	[SerializeField]
	private bool isPoisoned;

	[SerializeField]
	private Transform spawnPoint;
	
	[SerializeField] private EventManager eventManager;
	
	private void Awake()
	{
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
		eventManager.OnTimerStop.AddListener(() => gameObject.SetActive(false));
	}
	
	private void Start()
	{
		transform.localPosition = spawnPoint.localPosition;
	}

	public void BecomePoisoned(float poisonTime)
	{
		isPoisoned = true;
		PlayerMovement playerMovement = GetComponent<PlayerMovement>();
		playerMovement.SetRunSpeed(playerMovement.GetRunSpeed() / 1.5f);
		playerMovement.SetWalkSpeed(playerMovement.GetWalkSpeed() / 1.5f);
		Die(poisonTime);
	}

	public void Die(float timeDelay = 0f)
	{
		StartCoroutine(DieCoroutine(timeDelay));
	}

	private IEnumerator DieCoroutine(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		eventManager.OnAiScore.Invoke();
		transform.localPosition = new Vector2(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
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
		if (other.gameObject.GetComponent<PoisonCloud>())
		{
			timeInPoison = 0f;
		}
	}

	public bool IsPoisoned()
	{
		return isPoisoned;
	}
}
