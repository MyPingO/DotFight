using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerAbilitiesBase : MonoBehaviour
{
	[SerializeField] protected float MainAbilityCooldown { get; set; }
	[SerializeField] protected float MainAbilityDuration { get; set; }
	[SerializeField] protected float currentMainAbilityCooldown;
	[SerializeField] protected float SecondaryAbilityCooldown { get; set; }
	[SerializeField] protected float SecondaryAbilityDuration { get; set; }
	[SerializeField] protected float currentSecondaryAbilityCooldown;
	[TextArea (3, 10)] [SerializeField] protected float mainAbilityDescription;
	[TextArea (3, 10)] [SerializeField] protected float secondaryAbilityDescription;
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
			currentMainAbilityCooldown = MainAbilityCooldown;
		}

		if (Input.GetMouseButtonDown(1) && currentSecondaryAbilityCooldown <= 0)
		{
			CastSecondaryAbility();
			currentSecondaryAbilityCooldown = SecondaryAbilityCooldown;
		}
	}

	protected virtual Vector2 GetMouseDirectionNormalized()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
		return direction;
		
	}
	
}
