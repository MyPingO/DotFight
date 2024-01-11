using UnityEngine;

public class DotAttributes
{
    public string name;
    public string color;
    public string mainAbilityName;
    public string secondaryAbilityName;
    public Sprite mainAbilitySprite;
    public Sprite secondaryAbilitySprite;

    // create constructor
    public DotAttributes(string name, string color, string mainAbilityName, string secondaryAbilityName, Sprite mainAbilitySprite, Sprite secondaryAbilitySprite)
    {
        this.name = name;
        this.color = color;
        this.mainAbilityName = mainAbilityName;
        this.secondaryAbilityName = secondaryAbilityName;
        this.mainAbilitySprite = mainAbilitySprite;
        this.secondaryAbilitySprite = secondaryAbilitySprite;
    }
}
