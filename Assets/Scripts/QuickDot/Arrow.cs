using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
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
		if (windupTimer > 0) return;
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerBehaviour>().Die();
		}
		Destroy(gameObject);
	}
}
