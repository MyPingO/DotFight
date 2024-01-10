using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text playerName;
    public TMP_Text playerScore;
    public Image playerIcon;

    public TMP_Text aiName;
    public TMP_Text aiScore;
    public Image aiIcon;

    public GameObject gameManager;

    private EventManager eventManager;
    private GameObject player;
    private GameObject ai;

    private int playerScoreValue = 0;
    private int aiScoreValue = 0;

    void Awake()
    {

        eventManager = gameManager.GetComponent<EventManager>();
        //TODO: get also AI when it is implemented
        player = GameObject.FindGameObjectWithTag("Player");
        (string pName, string pColor) = getDotIstanceProperties(player);
        playerName.text = "Player "+ pName;
        playerIcon.color = ColorUtility.TryParseHtmlString(pColor, out Color color) ? color : Color.white;
        
    }

    private void OnEnable()
    {
        eventManager.OnPlayerScore.AddListener(UpdatePlayerScore);
        eventManager.OnAiScore.AddListener(UpdateAiScore);
    }

    private void OnDisable()
    {
        eventManager.OnPlayerScore.RemoveListener(UpdatePlayerScore);
        eventManager.OnAiScore.RemoveListener(UpdateAiScore);
    }

    private (string name, string color) getDotIstanceProperties(GameObject dot)
    {
        if (player.GetComponent<PlayerAbilitiesBase>() is QuickDotAbilities quickDot)
        {
            return (name: "QuickDot", color: "#FFFFFF");
        }
        else if (player.GetComponent<PlayerAbilitiesBase>() is FireDotAbilities fireDot)
        {
            return (name: "FireDot", color: "#FF2C00");
        }
        else if (player.GetComponent<PlayerAbilitiesBase>() is MagicDotAbilities magicDot)
        {
            return (name: "MagicDot", color: "#FFA7F9");
        }
        else if (player.GetComponent<PlayerAbilitiesBase>() is PoisonDotAbilities poisonDot)
        {
            return (name: "PoisonDot", color: "#00E852");
        }
        else return (name: "Dot", color: "#CCCCCC");
    }

    private void UpdatePlayerScore()
    {
        playerScoreValue++;
        playerScore.text = " × " + playerScoreValue;
    }

    private void UpdateAiScore()
    {
        aiScore.text = " × " + aiScoreValue;
    }
}
