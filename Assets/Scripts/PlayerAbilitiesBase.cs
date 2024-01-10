using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerAbilitiesBase : MonoBehaviour
{
	[SerializeField] protected float mainAbilityCooldown;
	[SerializeField] protected float mainAbilityDuration;
	[SerializeField] protected float currentMainAbilityCooldown;
	[SerializeField] protected float secondaryAbilityCooldown;
	[SerializeField] protected float secondaryAbilityDuration;
	[SerializeField] protected float currentSecondaryAbilityCooldown;
	[TextArea (3, 10)] protected float mainAbilityDescription;
	[TextArea (3, 10)] protected float secondaryAbilityDescription;
	public UnityEvent onMainAbilityCast;
	public UnityEvent onSecondaryAbilityCast;
	protected abstract void CastMainAbility();
	protected abstract void CastSecondaryAbility();
	
	protected virtual void Start() 
	{
		currentMainAbilityCooldown = 0;
		currentSecondaryAbilityCooldown = 0;
	}
	
	protected virtual void Update() 
	{
		currentMainAbilityCooldown = Mathf.Max(0, currentMainAbilityCooldown - Time.deltaTime);
		currentSecondaryAbilityCooldown = Mathf.Max(0, currentSecondaryAbilityCooldown - Time.deltaTime);
		
		if (Input.GetMouseButtonDown(0) && currentMainAbilityCooldown <= 0)
		{
			CastMainAbility();
			currentMainAbilityCooldown = mainAbilityCooldown;
		}

		if (Input.GetMouseButtonDown(1) && currentSecondaryAbilityCooldown <= 0)
		{
			CastSecondaryAbility();
			currentSecondaryAbilityCooldown = secondaryAbilityCooldown;
		}
	}

	protected virtual Vector2 GetMouseDirectionNormalized()
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
