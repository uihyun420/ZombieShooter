using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    public GameObject GameOverUi;
    public Text scoreText;
    public Text ammoText;

    public Gun gun;

    public int score;
    public bool isGameOver { get; set; }
    

    private void Awake()
    {
        isGameOver = false;
        GameOverUi.SetActive(false);
        score = 0;
    }
    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ScoreText()
    {
        scoreText.text = "Scroe: " + score;
    }

    public void AmmoText()
    {
        ammoText.text = ($"{gun.magAmmo}/{gun.ammoRemain}");
    }
    public void OnPlayerDead()
    {
        isGameOver = true;
        GameOverUi.SetActive(true);
    }

    private void Update()
    {
        AmmoText();
        ScoreText();
    }
    public void AddScore(int amount)
    {
        if(!isGameOver)
        {
            score += amount;
            Debug.Log("10Á¡ È¹µæ");
        }     
    }

    
}
