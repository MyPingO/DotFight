using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : Danger
{
	[SerializeField] private float duration;
	private void Start()
	{
		DestroyDanger(duration);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		if (other.gameObject.CompareTag("Player") && other.gameObject != caster)
		{
			if (other.gameObject.TryGetComponent(out AIDot aiDot))
			{
				aiDot.Die();
			}
			else other.gameObject.GetComponent<PlayerBehaviour>().Die();
		}
	}
	
}
