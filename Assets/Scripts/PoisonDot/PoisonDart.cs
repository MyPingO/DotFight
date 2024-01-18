using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PoisonDart : Danger
{
	[SerializeField] private float force = 10f;
	[SerializeField] private float poisonDuration = 5f;
	private Rigidbody2D rigidBody;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}
	private void Start()
	{
		rigidBody.AddForce(transform.up * force, ForceMode2D.Impulse);
	}
	
	private void Update()
	{
		// rotate the dart to face the direction it is moving
		transform.rotation = Quaternion.LookRotation(Vector3.forward, rigidBody.velocity);
		
	}
	
	new private void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.gameObject == caster) return;
		
		if (other.TryGetComponent(out DotBehavior dot))
		{
			dot.BecomePoisoned(poisonDuration);
		}
		DestroyDanger();
	}
}