using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreSection : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text playerName;
    public TMP_Text playerScore;
    public Image playerIcon;

    public TMP_Text aiName;
    public TMP_Text aiScore;
    public Image aiIcon;

    private GameObject gameManager;
    private EventManager eventManager;
    private GameObject player;
    private GameObject ai;

    private int playerScoreValue = 0;
    private int aiScoreValue = 0;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        eventManager = gameManager.GetComponent<EventManager>();
        //TODO: get also AI when it is implemented
        player = GameObject.FindGameObjectWithTag("Player");
        DotAttributes playerAttributes = gameManager.GetComponent<GameManager>().GetDotAttributes(player);
        playerName.text = "Player " + playerAttributes.name;
        playerIcon.color = ColorUtility.TryParseHtmlString(playerAttributes.color, out Color color) ? color : Color.white;

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
