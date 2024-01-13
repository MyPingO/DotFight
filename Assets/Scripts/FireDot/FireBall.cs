using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Danger
{
	[SerializeField] private float speed;

	private void Update()
	{
		transform.Translate(speed * Time.deltaTime * transform.up, Space.World);
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
		DestroyDanger();
	}
}
