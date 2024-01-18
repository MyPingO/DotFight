using UnityEngine;
using Unity.MLAgents;
using UnityEngine.Events;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AIDot : Agent
{
	[SerializeField]
	private DotAbilitiesBase abilities;
	
	[SerializeField]
	Rigidbody2D rigidBody;

	[SerializeField]
	private GameObject target;

	[SerializeField] private EventManager eventManager;
	public UnityEvent OnAbilityHitPlayer;

	public override void Initialize()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		abilities = GetComponent<DotAbilitiesBase>();
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
		eventManager.OnTimerStop.AddListener(() => gameObject.SetActive(false));
	}
	
	private void Start()
	{
		target = FindObjectOfType<PlayerDot>().gameObject;
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
	[SerializeField] private float mouseX;
	[SerializeField] private float mouseY;
	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// int moveXInput = actionBuffers.DiscreteActions[0] - 1;
		// jumpRequested = actionBuffers.DiscreteActions[1] == 1;
		// bool leftShiftPressed = actionBuffers.DiscreteActions[2] == 1;
		int mouseButton = actionBuffers.DiscreteActions[0];
		mouseX = actionBuffers.ContinuousActions[0];
		mouseY = actionBuffers.ContinuousActions[1];

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

	public GameObject GetTarget()
	{
		return target;
	}
	
	public Vector2 GetMouseAimDirectionNormalized()
	{
		return new Vector2(mouseX, mouseY).normalized;
	}
	
	public Vector2 GetMouseAimCoordinates()
	{
		return new Vector2(mouseX, mouseY);
	}
}
