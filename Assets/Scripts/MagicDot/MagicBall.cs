using Unity.VisualScripting;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float speed;
	[SerializeField] private float lifeTime;
	[SerializeField] private GameObject caster;
	
	private void Start()
	{
		Destroy(gameObject, lifeTime);
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
	
	public void SetCaster(GameObject caster)
	{
		this.caster = caster;
	}
	
	public void LifeTime(float time)
	{
		lifeTime = time;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject == caster) return;
		
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerBehaviour>().Die();
		}
		Destroy(gameObject);
	}
}
