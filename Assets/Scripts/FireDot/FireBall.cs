using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
	[SerializeField] private float speed;

	private void Update()
	{
		transform.Translate(speed * Time.deltaTime * transform.up, Space.World);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerBehaviour>().Die();
		}
		Destroy(gameObject);
	}
}
