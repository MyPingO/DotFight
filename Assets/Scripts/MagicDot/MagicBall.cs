using UnityEngine;

public class MagicBall : Danger
{
	[SerializeField]
	private Transform target;

	[SerializeField]
	private float speed;

	[SerializeField]
	private Rigidbody2D rigidBody;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		rigidBody.velocity = speed * (target.position - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation(Vector3.forward, rigidBody.velocity);
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	private new void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.gameObject == caster)
			return;

		if (other.TryGetComponent(out DotBehavior dot))
		{
			dot.Die();
		}
		Destroy(gameObject);
	}
}
