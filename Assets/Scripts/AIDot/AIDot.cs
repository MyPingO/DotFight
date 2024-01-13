using UnityEngine;
using Unity.MLAgents;
using UnityEngine.Events;
using System.Collections;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AIDot : Agent
{
	[SerializeField]
	AIMovement aiMovement;

	[SerializeField]
	Rigidbody2D rigidBody;

	[SerializeField]
	private AIDotAbilitiesBase abilities;

	[SerializeField]
	private GameObject target;

	[SerializeField] private Transform spawnPoint;
	public UnityEvent OnAbilityHitPlayer;

	public override void Initialize()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		aiMovement = GetComponent<AIMovement>();
		abilities = GetComponent<AIDotAbilitiesBase>();
		
		transform.localPosition = spawnPoint.localPosition;
	}

	public override void OnEpisodeBegin()
	{
		// randomize position (-10, 10) on X, and (-5, 5) on Y
		// transform.localPosition = new Vector2(Random.Range(-4.5f, 4.5f), Random.Range(-5f, 5f));
		rigidBody.velocity = Vector2.zero;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation((Vector2)transform.localPosition); //x2
		sensor.AddObservation((Vector2)target.transform.localPosition); //x2
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		// actionsOut.DiscreteActions.Array[0] = (int)Input.GetAxisRaw("Horizontal") + 1;
		// actionsOut.DiscreteActions.Array[1] = Input.GetButton("Jump") ? 1 : 0;
		// actionsOut.DiscreteActions.Array[2] = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
		actionsOut.DiscreteActions.Array[0] = Input.GetMouseButton(0)
			? 0
			: Input.GetMouseButton(1)
				? 1
				: 2;
		actionsOut.ContinuousActions.Array[0] = Input.mousePosition.x;
		actionsOut.ContinuousActions.Array[1] = Input.mousePosition.y;
	}

	public float angleThreshold = 20f;

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// int moveXInput = actionBuffers.DiscreteActions[0] - 1;
		// jumpRequested = actionBuffers.DiscreteActions[1] == 1;
		// bool leftShiftPressed = actionBuffers.DiscreteActions[2] == 1;
		int mouseButton = actionBuffers.DiscreteActions[0];
		float mouseX = actionBuffers.ContinuousActions[0];
		float mouseY = actionBuffers.ContinuousActions[1];

		// // --LOOKING-- //
		abilities.SetAIMousePosition(mouseX, mouseY);
		abilities.SetMouseButtonRecieved(mouseButton);

		// // --REWARDS-- //
		// // Calculate the aiming direction
		// Vector2 mousePosition = abilities.GetAIMousePosition();
		// Vector2 aimDirection = (mousePosition - (Vector2)transform.position).normalized;

		// // Calculate the direction to the target
		// Vector2 targetDirection = (
		// 	target.transform.localPosition - transform.localPosition
		// ).normalized;

		// // Calculate the angle between the aim direction and target direction
		// float angle = Vector2.Angle(aimDirection, targetDirection);

		// // Reward logic based on angle // Define a suitable threshold for your game
		// float rewardForPerfectAiming = 10f; // Define a reward value for accurate aiming
		// float penaltyForInaccurateAiming = -4f; // Define a penalty for inaccurate aiming

		// if (angle < angleThreshold)
		// {
		// 	float scaledReward = (angleThreshold - angle) / angleThreshold * rewardForPerfectAiming;
		// 	AddReward(scaledReward);
		// 	print("Got Reward For Accurate Aiming");
		// }
		// else
		// {
		// 	// Scaled penalty based on how much the angle exceeds the threshold
		// 	float penaltyScale = (angle - angleThreshold) / (180f - angleThreshold); // Assuming the max angle deviation is 180 degrees
		// 	float scaledPenalty = penaltyScale * penaltyForInaccurateAiming;
		// 	AddReward(scaledPenalty);
		// 	print("Got Penalty For Inaccurate Aiming Angle");
		// }

		// --ABILITIES-- //

		if (mouseButton == 0)
		{
			abilities.TriggerMainAbility();
		}
	}

	// --BEHAVIOUR-- //
	[SerializeField]
	private float timeInPoison;

	[SerializeField]
	private float timeUntilPoisonFatal;

	[SerializeField]
	private bool isPoisoned;

	public void BecomePoisoned(float poisonTime)
	{
		isPoisoned = true;
		aiMovement.SetRunSpeed(aiMovement.GetRunSpeed() / 1.5f);
		aiMovement.SetWalkSpeed(aiMovement.GetWalkSpeed() / 1.5f);
		Die(poisonTime);
	}

	public void Die(float timeDelay = 0f)
	{
		// end episode mlagent
		if (timeDelay != 0f)
		{
			StartCoroutine(DieAfterDelay(timeDelay));
		}
		else
		{
			SetRandomPosition();
			EndEpisode();
		}
	}

	private IEnumerator DieAfterDelay(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		SetRandomPosition();
		EndEpisode();
	}

	private void SetRandomPosition()
	{
		transform.localPosition = new Vector2(Random.Range(-10f, 10f), Random.Range(-5.5f, 5.5f));
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Poison"))
		{
			timeInPoison += Time.deltaTime;
			// print("Lost Reward For Staying In Poison");
			if (timeInPoison >= timeUntilPoisonFatal)
			{
				Die();
			}
		}
	}

	public GameObject GetTarget()
	{
		return target;
	}
}
