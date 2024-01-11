using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitiesSection : MonoBehaviour
{
    public Slider mainAbilitySlider;
    public Slider secondaryAbilitySlider;

    public TMP_Text mainAbilityName;
    public TMP_Text secondaryAbilityName;

    public Image mainAbilityImage;
    public Image secondaryAbilityImage;

    private GameObject gameManager;
    private GameObject player;
    private float mainAbilityCooldownEnd;
    private float secondaryAbilityCooldownEnd;
    void Awake()
    {
        Debug.Log("Awake");
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        player = GameObject.FindGameObjectWithTag("Player");
        DotAttributes playerAttributes = gameManager.GetComponent<GameManager>().GetDotAttributes(player);

        mainAbilityName.text = playerAttributes.mainAbilityName;
        secondaryAbilityName.text = playerAttributes.secondaryAbilityName;
        mainAbilityName.color = ColorUtility.TryParseHtmlString(playerAttributes.color, out Color color) ? color : Color.white;
        secondaryAbilityName.color = ColorUtility.TryParseHtmlString(playerAttributes.color, out Color color2) ? color2 : Color.white;

        mainAbilityImage.sprite = playerAttributes.mainAbilitySprite;
        secondaryAbilityImage.sprite = playerAttributes.secondaryAbilitySprite;

        //change slider fill color to match player color
        mainAbilitySlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = ColorUtility.TryParseHtmlString(playerAttributes.color, out Color color3) ? color3 : Color.white;
        secondaryAbilitySlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = ColorUtility.TryParseHtmlString(playerAttributes.color, out Color color4) ? color4 : Color.white;


    }

    private void OnEnable()
    {
        player.GetComponent<PlayerAbilitiesBase>().onMainAbilityCast.AddListener(HandleCastPrimaryAbility);
        player.GetComponent<PlayerAbilitiesBase>().onSecondaryAbilityCast.AddListener(HandleCastSecondaryAbility);
    }

    private void OnDisable()
    {
        player.GetComponent<PlayerAbilitiesBase>().onMainAbilityCast.RemoveListener(HandleCastPrimaryAbility);
        player.GetComponent<PlayerAbilitiesBase>().onSecondaryAbilityCast.RemoveListener(HandleCastSecondaryAbility);
    }

    // Update is called once per frame
    void Update()
    {
        //update the cooldown slider based on the time left from 0 to 1
        mainAbilitySlider.value = 1 - (mainAbilityCooldownEnd - Time.time) / player.GetComponent<PlayerAbilitiesBase>().GetMainAbilityCooldown();
        secondaryAbilitySlider.value = 1 - (secondaryAbilityCooldownEnd - Time.time) / player.GetComponent<PlayerAbilitiesBase>().GetSecondaryAbilityCooldown();
    }

    private void HandleCastPrimaryAbility()
    {
        float mainCooldown = player.GetComponent<PlayerAbilitiesBase>().GetMainAbilityCooldown();
        mainAbilityCooldownEnd = Time.time + mainCooldown;
    }

    private void HandleCastSecondaryAbility()
    {
        float secondaryCooldown = player.GetComponent<PlayerAbilitiesBase>().GetSecondaryAbilityCooldown();
        secondaryAbilityCooldownEnd = Time.time + secondaryCooldown;
    }
}
