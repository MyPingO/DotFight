using System.Collections;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
	[SerializeField] private float gracePeriodTime;
	[SerializeField] private float growthTime;
	[SerializeField] private float growthRate;
	[SerializeField] private float duration;
	public bool isPoisonous;
	
	void Awake()
	{
		isPoisonous = false;
		// disable the collider until the cloud is ready to spread
		GetComponent<Collider2D>().enabled = false;
	}
	void Start()
	{
		StartCoroutine(Activate());
	}
	
	private IEnumerator Initiate()
	{
		float timeElapsed = 0f;
		
		while (timeElapsed < gracePeriodTime)
		{
			timeElapsed += Time.deltaTime;
			yield return null;
		}
		
		isPoisonous = true;
		GetComponent<Collider2D>().enabled = true;
	}
	
	private IEnumerator Spread()
	{
		float timeElapsed = 0f;
		
		while (timeElapsed < growthTime)
		{
			transform.localScale += growthRate * Time.deltaTime * Vector3.one;
			timeElapsed += Time.deltaTime;
			yield return null;
		}
	}
	
	private IEnumerator Activate()
	{
		yield return Initiate();
		StartCoroutine(Spread());
		Destroy(gameObject, duration);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (isPoisonous && other.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerBehaviour>().BecomePoisoned(duration);
		}
	}
	
	public void SetDuration(float duration)
	{
		this.duration = duration;
	}
}
