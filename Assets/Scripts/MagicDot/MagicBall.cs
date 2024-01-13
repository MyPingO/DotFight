using Unity.VisualScripting;
using UnityEngine;

public class MagicBall : Danger
{
	[SerializeField] private Transform target;
	[SerializeField] private float speed;
	[SerializeField] private float lifeTime;
	
	private void Start()
	{
		DestroyDanger(lifeTime);
	}

	private void Update()
	{
		if (target == null) {
			Destroy(gameObject);
			return;
		}

		transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		lifeTime -= Time.deltaTime;
	}
	
	public void SetTarget(Transform target)
	{
		this.target = target;
	}
	
	
	public void LifeTime(float time)
	{
		lifeTime = time;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.gameObject == caster) return;
		
		if (other.CompareTag("Player"))
		{
			if (other.TryGetComponent(out AIDot aiDot))
			{
				aiDot.Die();
			}
			else other.GetComponent<PlayerBehaviour>().Die();
		}
		Destroy(gameObject);
	}
}
