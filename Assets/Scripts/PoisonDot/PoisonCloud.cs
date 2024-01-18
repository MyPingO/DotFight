using System.Collections;
using UnityEngine;

public class PoisonCloud : Danger
{
	[SerializeField] private float gracePeriodTime;
	[SerializeField] private float growthTime;
	[SerializeField] private float growthRate;
	[SerializeField] private float duration;
	public bool isPoisonous;
	
	private void Awake()
	{
		isPoisonous = false;
		// disable the collider until the cloud is ready to spread
		GetComponent<Collider2D>().enabled = false;
	}
	private void Start()
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
		DestroyDanger(duration);
	}
	
	new private void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.gameObject == caster) return;
		if (isPoisonous && other.TryGetComponent(out DotBehavior dot))
		{
			dot.BecomePoisoned(duration);
		}
	}
	
	public void SetDuration(float duration)
	{
		this.duration = duration;
	}
}
