using TMPro;
using UnityEngine;
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

	private int playerScoreValue = 0;
	private int aiScoreValue = 0;

	void Awake()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameController");
		eventManager = gameManager.GetComponent<EventManager>();
		player = FindObjectOfType<PlayerDot>().gameObject;
		DotAttributes playerAttributes = gameManager.GetComponent<GameManager>().GetDotAttributes(player);
		playerName.text = "You";
		playerIcon.color = ColorUtility.TryParseHtmlString(playerAttributes.color, out Color color) ? color : Color.white;
		aiName.text = "AI";
		aiIcon.color = ColorUtility.TryParseHtmlString("#FF2C00", out Color aiColor) ? aiColor : Color.white;

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
		playerScore.text = "x " + playerScoreValue;
	}

	private void UpdateAiScore()
	{
		aiScoreValue++;
		aiScore.text = "x " + aiScoreValue;
	}
	
	public int GetPlayerScore()
	{
		return playerScoreValue;
	}
	
	public int GetAiScore()
	{
		return aiScoreValue;
	}
}
