using UnityEngine;

public class FireTrail : Danger
{
	[SerializeField] private float duration;
	private void Start()
	{
		DestroyDanger(duration);
	}

	new private void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.gameObject == caster) return;
		if (other.TryGetComponent(out DotBehavior dotBehavior))
		{
			dotBehavior.Die();
		}
	}
	
}
