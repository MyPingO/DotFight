using UnityEngine;

public class GameManager : MonoBehaviour
{
	private EventManager eventManager;
	public GameObject FireDot;
	public GameObject MagicDot;
	public GameObject PoisonDot;
	public GameObject QuickDot;
	
	public float gameTimer = 90f;

	public DotAttributes GetDotAttributes(GameObject dot)
	{
		Sprite mainAbilitySprite;
		Sprite secondaryAbilitySprite;
		if (dot.GetComponent<DotAbilitiesBase>() is QuickDotAbilities)
		{
			mainAbilitySprite = Resources.Load<Sprite>("Sprites/ArrowIcon");
			secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/SpeedIcon");
			return new DotAttributes(name: "QuickDot", color: "#FFFFFF", mainAbilityName: "Arrow", secondaryAbilityName: "Speed", mainAbilitySprite, secondaryAbilitySprite);
		}
		else if (dot.GetComponent<DotAbilitiesBase>() is FireDotAbilities)
		{
			mainAbilitySprite = Resources.Load<Sprite>("Sprites/FireballIcon");
			secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/ScorchIcon");
			return new DotAttributes(name: "FireDot", color: "#FF2C00", mainAbilityName: "Fireball", secondaryAbilityName: "Firetrail", mainAbilitySprite, secondaryAbilitySprite);
		}
		else if (dot.GetComponent<DotAbilitiesBase>() is MagicDotAbilities)
		{
			mainAbilitySprite = Resources.Load<Sprite>("Sprites/SpellIcon");
			secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/ShapeBarrierIcon");
			return new DotAttributes(name: "MagicDot", color: "#FFA7F9", mainAbilityName: "Magic Ball", secondaryAbilityName: "Barrier", mainAbilitySprite, secondaryAbilitySprite);
		}
		else if (dot.GetComponent<DotAbilitiesBase>() is PoisonDotAbilities)
		{

			mainAbilitySprite = Resources.Load<Sprite>("Sprites/PoisonDartIcon");
			secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/PoisonGasIcon");
			return new DotAttributes(name: "PoisonDot", color: "#00E852", mainAbilityName: "Dart", secondaryAbilityName: "Poison Gas", mainAbilitySprite, secondaryAbilitySprite);
		}
		else
		{
			mainAbilitySprite = Resources.Load<Sprite>("Sprites/ArrowIcon");
			secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/SpeedIcon");
			return new DotAttributes(name: "Dot", color: "#CCCCCC", mainAbilityName: "None", secondaryAbilityName: "None", mainAbilitySprite, secondaryAbilitySprite);
		};
	}
	private void Awake()
	{
		eventManager = GetComponent<EventManager>();
		eventManager.OnTimerSet.Invoke(gameTimer);
		eventManager.OnTimerStart.Invoke();
		
		// Find what character the player chose is playerprefs
		string playerCharacter = PlayerPrefs.GetString("SelectedDot");
		
		// Set active the player character in the scene
		switch (playerCharacter)
		{
			case "FireDot":
				FireDot.SetActive(true);
				break;
			case "MagicDot":
				MagicDot.SetActive(true);
				break;
			case "PoisonDot":
				PoisonDot.SetActive(true);
				break;
			case "QuickDot":
				QuickDot.SetActive(true);
				break;
			default:
				Debug.LogWarning("No player character selected");
				break;
		}
		
	}
	
	private void OnApplicationQuit()
	{
		PlayerPrefs.DeleteKey("SelectedDot");
		PlayerPrefs.DeleteKey("SelectedMap");
	}
	
	public void LoadScene(string sceneName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}
	
	public void ResetTimeScale()
	{
		Time.timeScale = 1;
	}
}
