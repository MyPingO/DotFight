using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{
	public bool jumpIfMovingLeft = false;
	public bool jumpIfMovingRight = false;
	public float randomize;

	void Start()
	{
		GetComponent<Renderer>().enabled = false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		if (GetComponent<BoxCollider2D>() != null)
		{
			BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
			Gizmos.matrix = Matrix4x4.TRS(
				transform.position,
				transform.rotation,
				transform.lossyScale
			);
			Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
		}
	}
}
