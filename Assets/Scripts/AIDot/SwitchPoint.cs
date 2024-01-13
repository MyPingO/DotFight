using UnityEngine;

public class SwitchPoint : MonoBehaviour
{
	public float randomize;
	
	void Start()
	{
		GetComponent<Renderer>().enabled = false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
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
