using System.Collections;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
	public float teleportInterval = 10f;
	public float moveRangeX = 10f;
	public float moveRangeY = 5.5f;
	public bool startMoving = false;
	public bool startTeleporting = false;
	private Vector3 initialPosition;

	// Start is called before the first frame update
	void Start()
	{
		initialPosition = transform.localPosition;
		StartCoroutine(TeleportRandomly());
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			startTeleporting = !startTeleporting;
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			startMoving = !startMoving;
		}
		if (startMoving)
		MoveWithinRange();
	}

	IEnumerator TeleportRandomly()
	{
		while (true)
		{
			if (startTeleporting) 
			{
				Teleport();
			}
			yield return new WaitForSeconds(teleportInterval);
		}
	}

	void Teleport()
	{
		float randomX = Random.Range(-moveRangeX, moveRangeX);
		float randomY = Random.Range(-moveRangeY, moveRangeY);
		transform.localPosition = new Vector2(0f , 0f) + new Vector2(randomX, randomY);
	}

	void MoveWithinRange()
	{
		float moveSpeed = 5f;
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		Vector2 movement = new Vector2(horizontalInput, verticalInput) * moveSpeed * Time.deltaTime;
		
		Vector2 newPosition = (Vector2)transform.localPosition + movement;
		newPosition.x = Mathf.Clamp(newPosition.x, -moveRangeX, moveRangeX);
		newPosition.y = Mathf.Clamp(newPosition.y, -moveRangeY, moveRangeY);
		
		transform.localPosition = newPosition;
	}
}
