using System.Collections;
using System.Collections.Generic;
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

	void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);
		
		if (windupTimer > 0) return;
		if (other.gameObject.CompareTag("Player"))
		{
			if (other.gameObject.TryGetComponent(out AIDot aiDot))
			{
				aiDot.Die();
			}
			else other.gameObject.GetComponent<PlayerBehaviour>().Die();
		}
		DestroyDanger();
	}
}
