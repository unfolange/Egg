using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI gameOver;
    public GameObject restartButton;
    public GameObject mainMenuButton;
    public bool isGameActive;

    private GameObject player;
    [HideInInspector] public bool isGameOver = false;

    private int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        UpdateScore(0);
        isGameActive = true;
    }
    // Update is called once per frame

    void Awake()
    {
        player = GameObject.Find("EggPlayer");
    }
    void Update()
    {
    }
    public void UpdateScore(int scoreToAdd)
    {
        scoreText.text = "Score: " + scoreToAdd;
    }
    public void UpdateLife(float lifeAdd)
    {
        lifeText.text = "Life: " + lifeAdd;
    }
    // public void GameOver()
    // {
    //     gameOver.gameObject.SetActive(true);
    //     isGameActive = false;
    // }

    public void GameOver()
    {
        gameOver.gameObject.SetActive(true);
        isGameActive = false;
        isGameOver = true;

        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

}
