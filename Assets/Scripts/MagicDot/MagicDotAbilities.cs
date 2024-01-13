using UnityEngine;

public class MagicDotAbilities : PlayerAbilitiesBase
{
	[SerializeField] private GameObject MagicBallPrefab;
	[SerializeField] private GameObject BarrierPrefab;
	[SerializeField] private Transform target;
	protected override void CastMainAbility()
	{
		print("MagicDot main ability");
		SpawnMagicBall();
	}
	
	private void SpawnMagicBall() 
	{
		GameObject magicBallGO = Instantiate(MagicBallPrefab, transform.position, Quaternion.identity);
		MagicBall magicBall = magicBallGO.GetComponent<MagicBall>();
		magicBall.SetTarget(target);
		magicBall.SetCaster(gameObject);
		magicBall.LifeTime(mainAbilityDuration);
	}
	private void GetClosestTarget(out Transform target)
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		GameObject closestPlayer = null;
		float closestDistance = Mathf.Infinity;

		foreach (GameObject player in players)
		{
			if (player != gameObject) // Exclude the current player who is casting the spell
			{
				float distance = Vector3.Distance(transform.position, player.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestPlayer = player;
				}
			}
		}

		if (closestPlayer) target = closestPlayer.transform;
		else target = null;
	}

	protected override void CastSecondaryAbility()
	{
		print("MagicDot secondary ability");
		SpawnBarrier();
	}
	
	private void SpawnBarrier() 
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		GameObject magicBarrier = Instantiate(BarrierPrefab, mousePosition, Quaternion.identity);
		
		OrientMagicBarrier(magicBarrier, mousePosition);
		SetFlatSideTag(magicBarrier, mousePosition);
		
		Destroy(magicBarrier, secondaryAbilityDuration);
	}
	
	private void SetFlatSideTag(GameObject magicBarrier, Vector2 mousePosition)
	{
		GameObject flatSide = magicBarrier.transform.Find("FlatSide").gameObject;
		
		float mouseHorizontalDistanceFromPlayer = Mathf.Abs(mousePosition.x - transform.position.x);
		
		//Check if mouse click is close to under the player
		if (mousePosition.y < transform.position.y && mouseHorizontalDistanceFromPlayer <= 3)
		{
			flatSide.tag = "Ground";
		}
		// Check if mouse click is close to above the player
		else if (mousePosition.y > transform.position.y && mouseHorizontalDistanceFromPlayer <= 1.5f)
		{
			flatSide.tag = "Ground";
		}
		// If mouse click is to the sides of the player
		else
		{
			flatSide.tag = "Wall";
		}
		
	}
	private void OrientMagicBarrier(GameObject magicBarrier, Vector2 mousePosition)
	{
		float mouseHorizontalDistanceFromPlayer = Mathf.Abs(mousePosition.x - transform.position.x);
		
		//Check if mouse click is close to under the player 
		if (mousePosition.y < transform.position.y && mouseHorizontalDistanceFromPlayer <= 5)
		{
			magicBarrier.transform.eulerAngles = new Vector3(0, 0, 0);
		}
		// Check if mouse click is close to above the player
		else if (mousePosition.y > transform.position.y && mouseHorizontalDistanceFromPlayer <= 1.5f)
		{
			magicBarrier.transform.eulerAngles = new Vector3(0, 0, 180);
		}
		// If mouse click is to the sides of the player
		else
		{
			if (mousePosition.x > transform.position.x) magicBarrier.transform.eulerAngles = new Vector3(0, 0, 90);
			else magicBarrier.transform.eulerAngles = new Vector3(0, 0, -90);
		}
	}

	protected override void Update()
	{
		currentMainAbilityCooldown = Mathf.Max(0, currentMainAbilityCooldown - Time.deltaTime);
		currentSecondaryAbilityCooldown = Mathf.Max(0, currentSecondaryAbilityCooldown - Time.deltaTime);
		
		if (Input.GetMouseButtonDown(0))
		{
			// Only cast main ability if there is a target
			GetClosestTarget(out target);
			if (target != null)
			{
				TriggerMainAbility();
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			TriggerSecondaryAbility();
		}
	}
}
