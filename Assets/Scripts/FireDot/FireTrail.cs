using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
	[SerializeField] private float duration;
	[SerializeField] private GameObject caster;
	private void Start()
	{
		Destroy(gameObject, duration);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player") && other.gameObject != caster)
		{
			other.gameObject.GetComponent<PlayerBehaviour>().Die();
		}
	}
	
	public void SetCaster(GameObject caster)
	{
		this.caster = caster;
	}
	
}
