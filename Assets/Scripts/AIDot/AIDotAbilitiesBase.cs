using UnityEngine;
using UnityEngine.Events;

public abstract class AIDotAbilitiesBase : MonoBehaviour
{
	[SerializeField] protected AIMovement aiMovement;
	[SerializeField] protected float mainAbilityCooldown;
	[SerializeField] protected float mainAbilityDuration;
	[SerializeField] protected float currentMainAbilityCooldown;
	[SerializeField] protected float secondaryAbilityCooldown;
	[SerializeField] protected float secondaryAbilityDuration;
	[SerializeField] protected float currentSecondaryAbilityCooldown;
	[TextArea (3, 10)] protected float mainAbilityDescription;
	[TextArea (3, 10)] protected float secondaryAbilityDescription;
	public UnityEvent OnMainAbilityCast;
	public UnityEvent OnSecondaryAbilityCast;
	protected abstract void CastMainAbility();
	protected abstract void CastSecondaryAbility();
	
	protected float AIMouseX;
	protected float AIMouseY;
	protected int mouseButtonRecieved;
	
	protected virtual void Awake() 
	{
		aiMovement = GetComponent<AIMovement>();
	}
	protected virtual void Start() 
	{
		currentMainAbilityCooldown = 0;
		currentSecondaryAbilityCooldown = 0;
	}
	
	protected virtual void Update() 
	{
		currentMainAbilityCooldown = Mathf.Max(0, currentMainAbilityCooldown - Time.deltaTime);
		currentSecondaryAbilityCooldown = Mathf.Max(0, currentSecondaryAbilityCooldown - Time.deltaTime);
		
		if (GetMouseButtonRecieved() == 0)
		{
			TriggerMainAbility();
		}

		if (GetMouseButtonRecieved() == 1)
		{
			TriggerSecondaryAbility();
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
	
	public virtual void SetAIMousePosition(float x, float y)
	{
		AIMouseX = x;
		AIMouseY = y;
	}
	
	public virtual Vector2 GetAIMousePosition()
	{
		return new Vector2(transform.position.x + AIMouseX, transform.position.y + AIMouseY);
	}
	
	public virtual void SetMouseButtonRecieved(int button)
	{
		mouseButtonRecieved = button;
	}

	public virtual int GetMouseButtonRecieved()
	{
		return mouseButtonRecieved;
	}
	public virtual Vector2 GetMouseDirectionNormalized()
	{
		Vector2 mousePosition = GetAIMousePosition();
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
