using UnityEngine;
using UnityEngine.Events;

public class Danger : MonoBehaviour
{
	public UnityEvent OnDangerDestroyed;
	public GameObject caster;
	
	public void DestroyDanger(float delay = 0f)
	{
		OnDangerDestroyed.Invoke();
		Destroy(gameObject, delay);
	}
	
	protected void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Dot") && other.gameObject != caster)
		{
			// For AI
			if (caster.GetComponent<AIDot>()) caster.GetComponent<AIDot>().OnAbilityHitPlayer.Invoke();
		}
	}
	
	public void SetCaster(GameObject caster)
	{
		this.caster = caster;
	}
}
