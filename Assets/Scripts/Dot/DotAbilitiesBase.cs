using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class DotAbilitiesBase : MonoBehaviour
{
	[SerializeField]
	private enum ControllerType
	{
		Player,
		AI
	}

	protected Func<Vector2> GetAimDirection;

	[SerializeField]
	private ControllerType controllerType;

	[SerializeField]
	protected float mainAbilityCooldown;

	[SerializeField]
	protected float mainAbilityDuration;

	[SerializeField]
	protected float currentMainAbilityCooldown;

	[SerializeField]
	protected float secondaryAbilityCooldown;

	[SerializeField]
	protected float secondaryAbilityDuration;

	[SerializeField]
	protected float currentSecondaryAbilityCooldown;

	[TextArea(3, 10)]
	protected float mainAbilityDescription;

	[TextArea(3, 10)]
	protected float secondaryAbilityDescription;
	public UnityEvent OnMainAbilityCast;
	public UnityEvent OnSecondaryAbilityCast;
	protected abstract void CastMainAbility();
	protected abstract void CastSecondaryAbility();

	[SerializeField]
	protected AudioClip mainAbilitySFX;

	[SerializeField]
	protected AudioClip secondaryAbilitySFX;

	protected void Awake()
	{
		print("Dot Abilities Base Awake");
		if (TryGetComponent(out AIDot ai))
		{
			print("Dot Abilities Base Awake: AI");
			controllerType = ControllerType.AI;
			SetAimDirectionFunction(ai.GetMouseAimDirectionNormalized);
		}
		else
		{
			print("Dot Abilities Base Awake: Player");
			controllerType = ControllerType.Player;
			SetAimDirectionFunction(GetPlayerMouseDirectionNormalized);
		}
	}

	private void SetAimDirectionFunction(Func<Vector2> aimDirectionFunction)
	{
		GetAimDirection = aimDirectionFunction;
	}

	protected virtual void Start()
	{
		currentMainAbilityCooldown = 0;
		currentSecondaryAbilityCooldown = 0;
		if (!AudioManager.instance)
		{
			Debug.LogWarning("No AudioManager found in scene");
			return;
		}
		OnMainAbilityCast.AddListener(() => AudioManager.instance.PlaySFX(mainAbilitySFX));
		OnSecondaryAbilityCast.AddListener(
			() => AudioManager.instance.PlaySFX(secondaryAbilitySFX)
		);
	}
	protected virtual void Update()
	{
		currentMainAbilityCooldown = Mathf.Max(0, currentMainAbilityCooldown - Time.deltaTime);
		currentSecondaryAbilityCooldown = Mathf.Max(
			0,
			currentSecondaryAbilityCooldown - Time.deltaTime
		);

		if (controllerType == ControllerType.Player)
		{
			if (Input.GetMouseButtonDown(0))
			{
				TriggerMainAbility();
			}

			if (Input.GetMouseButtonDown(1))
			{
				TriggerSecondaryAbility();
			}
		}
	}

	public void TriggerMainAbility()
	{
		if (currentMainAbilityCooldown <= 0)
		{
			CastMainAbility();
			OnMainAbilityCast.Invoke();
			currentMainAbilityCooldown = mainAbilityCooldown;
		}
	}

	public void TriggerSecondaryAbility()
	{
		if (currentSecondaryAbilityCooldown <= 0)
		{
			CastSecondaryAbility();
			OnSecondaryAbilityCast.Invoke();
			currentSecondaryAbilityCooldown = secondaryAbilityCooldown;
		}
	}

	protected virtual Vector2 GetPlayerMouseDirectionNormalized()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
		return direction;
	}

	public float GetMainAbilityCooldown()
	{
		return mainAbilityCooldown;
	}

	public void SetMainAbilityCooldown(float cooldown)
	{
		mainAbilityCooldown = cooldown;
	}

	public float GetMainAbilityDuration()
	{
		return mainAbilityDuration;
	}

	public void SetMainAbilityDuration(float duration)
	{
		mainAbilityDuration = duration;
	}

	public float GetSecondaryAbilityCooldown()
	{
		return secondaryAbilityCooldown;
	}

	public void SetSecondaryAbilityCooldown(float cooldown)
	{
		secondaryAbilityCooldown = cooldown;
	}

	public float GetSecondaryAbilityDuration()
	{
		return secondaryAbilityDuration;
	}

	public void SetSecondaryAbilityDuration(float duration)
	{
		secondaryAbilityDuration = duration;
	}
}
