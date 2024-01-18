using UnityEngine;

public class FireBall : Danger
{
	[SerializeField] private float speed;

	private void Update()
	{
		transform.Translate(speed * Time.deltaTime * transform.up, Space.World);
	}
	
	new private void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.gameObject == caster) return;
		
		if (other.CompareTag("Dot"))
		{
			other.GetComponent<DotBehavior>().Die();
		}
		DestroyDanger();
	}
}
