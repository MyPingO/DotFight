using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PoisonDart : MonoBehaviour
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
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerBehaviour>().BecomePoisoned(poisonDuration);
		}
		Destroy(gameObject);	
	}
}