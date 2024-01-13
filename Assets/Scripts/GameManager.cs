using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EventManager eventManager;

    public DotAttributes GetDotAttributes(GameObject dot)
    {
        Sprite mainAbilitySprite = null;
        Sprite secondaryAbilitySprite = null;
        if (dot.GetComponent<PlayerAbilitiesBase>() is QuickDotAbilities quickDot)
        {
            mainAbilitySprite = Resources.Load<Sprite>("Sprites/ArrowIcon");
            secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/SpeedIcon");
            return new DotAttributes(name: "QuickDot", color: "#FFFFFF", mainAbilityName: "Arrow", secondaryAbilityName: "Speed", mainAbilitySprite, secondaryAbilitySprite);
        }
        else if (dot.GetComponent<PlayerAbilitiesBase>() is FireDotAbilities fireDot)
        {
            mainAbilitySprite = Resources.Load<Sprite>("Sprites/FireballIcon");
            secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/ScorchIcon");
            return new DotAttributes(name: "FireDot", color: "#FF2C00", mainAbilityName: "FIreball", secondaryAbilityName: "Firetrail", mainAbilitySprite, secondaryAbilitySprite);
        }
        else if (dot.GetComponent<PlayerAbilitiesBase>() is MagicDotAbilities magicDot)
        {
            mainAbilitySprite = Resources.Load<Sprite>("Sprites/SpellIcon");
            secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/ShapeBarrierIcon");
            return new DotAttributes(name: "MagicDot", color: "#FFA7F9", mainAbilityName: "Magic Ball", secondaryAbilityName: "Shape", mainAbilitySprite, secondaryAbilitySprite);
        }
        else if (dot.GetComponent<PlayerAbilitiesBase>() is PoisonDotAbilities poisonDot)
        {

            mainAbilitySprite = Resources.Load<Sprite>("Sprites/PoisonDartIcon");
            secondaryAbilitySprite = Resources.Load<Sprite>("Sprites/SpeedIcon");
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
    }

    private void Start()
    {
        eventManager.OnTimerSet.Invoke(90);
        eventManager.OnTimerStart.Invoke();
    }
}
