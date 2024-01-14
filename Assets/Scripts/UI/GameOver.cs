using UnityEngine;
using TMPro;
using System;
public class GameOver : MonoBehaviour
{
	[SerializeField] private EventManager eventManager;
	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private GameObject darkPanel;
	[SerializeField] private TMP_Text playerScoreText;
	[SerializeField] private TMP_Text aiScoreText;
	[SerializeField] private TMP_Text winOrLoseText;
	[SerializeField] private ScoreSection scoreSection;
	private void Awake()
	{
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
	}
	
	void Start()
	{
		eventManager.OnTimerStop.AddListener(ShowGameOver);
	}

	private void ShowGameOver()
	{
		darkPanel.SetActive(true);
		gameOverPanel.SetActive(true);
		SetText();
	}
	
	private void SetText()
	{
		int playerScore = scoreSection.GetPlayerScore();
		int aiScore = scoreSection.GetAiScore();
		playerScoreText.text = playerScore.ToString();
		aiScoreText.text = aiScore.ToString();
		if (playerScore > aiScore)
		{
			winOrLoseText.text = "You Win!";
			winOrLoseText.color = Color.green;
		}
		else if (playerScore < aiScore)
		{
			winOrLoseText.text = "You Lose!";
			winOrLoseText.color = Color.red;
		}
		else
		{
			winOrLoseText.text = "Draw!";
			winOrLoseText.color = Color.yellow;
		}
	}
}
