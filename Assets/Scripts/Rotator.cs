using UnityEngine;

public class Rotator : MonoBehaviour
{
	// Speed of rotation in degrees per second
	public float rotationSpeed = 30f;

	private void Update()
	{
		// Rotate the game object around its z-axis (for 2D rotation)
		transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
		// If rotation is around 90 degrees, set Layer to Wall, else set it to Ground
		if (Mathf.Abs(transform.rotation.eulerAngles.z - 90f) < 20f)
		{
			gameObject.layer = LayerMask.NameToLayer("Wall");
		}
		else
		{
			gameObject.layer = LayerMask.NameToLayer("Ground");
		}
	}
}

