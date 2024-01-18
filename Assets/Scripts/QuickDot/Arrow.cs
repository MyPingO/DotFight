using UnityEngine;

public class Arrow : Danger
{
	[SerializeField] private float speed;
	[SerializeField] private float windupTimer;

	private void Update()
	{
		if (windupTimer <= 0) 
		{
			transform.Translate(speed * Time.deltaTime * transform.up, Space.World);
		}
		else windupTimer -= Time.deltaTime;
	}

	new void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		
		if (windupTimer > 0) return;
		if (other.TryGetComponent(out DotBehavior dot))
		{
			dot.Die();
		}
		DestroyDanger();
	}
}
