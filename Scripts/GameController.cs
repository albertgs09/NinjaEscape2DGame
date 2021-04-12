using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    float timeLeft = 1f;
    public float timerCount = 30;
    int points;
    int highScore;
    int failing;
    int light;
    int overLvl;
    public bool lightsOn;
    bool timeStart;
    bool timerStart;
    bool complete;
    bool lvlPlayed;
    public bool fail;
    public Text pointsText;
    public Text timer;
    SpriteRenderer backGround;
    public GameObject restart, time, failed, ghostMode, blurred;
    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        backGround = GetComponent<SpriteRenderer>();
        overLvl = PlayerPrefs.GetInt("Played");
        if(overLvl == 0)
        {
            lvlPlayed = false;
            StartCoroutine(Canvas());

        }
        lightsOn = true;
        time.SetActive(true);
        PlayerPrefs.SetInt("Lights", 1);
        PlayerPrefs.SetInt("Fail", 0);

    }

    // Update is called once per frame
    void Update()
    {
        light = PlayerPrefs.GetInt("Lights");
        failing = PlayerPrefs.GetInt("Fail");
        StartCoroutine(Canvas());
        Timer();
        ScoreText();
        if(light == 0)
        {
            backGround.color = new Color(.1f, .1f, .1f, 1f);
            timeStart = true;
        }

        if(timeStart && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }

        if(timeLeft < 0)
        {
            timeStart = false;
            timeLeft = 1f;
            //  lightsOn = true;
            PlayerPrefs.SetInt("Lights", 1);
            backGround.color = new Color(1f, 1f, 1f, 1f);
        }
        if(timerStart && timerCount > 0)
        {
            timerCount -= Time.deltaTime;
        }

        if(timerCount < 0)
        {
            timerStart = false;
            timerCount = 0;
            PlayerPrefs.SetInt("Fail", 1);
            //time out
        }

        points = PlayerPrefs.GetInt("Points", points);
    }


    void ScoreText()
    {
        if(points > highScore)
        {
            PlayerPrefs.SetInt("HighScore", points);
        }

        pointsText.text = "Points: " + points.ToString();
    }

    public void Restart()
    {
       
            Application.LoadLevel(Application.loadedLevel);
        
    }
    void Timer()
    {
        timer.text = " " + Mathf.RoundToInt(timerCount);
    }

    IEnumerator Canvas()
    {
        if (failing == 1)
        {
            yield return new WaitForSeconds(1.0f);
            restart.SetActive(true);
            failed.SetActive(true);
            time.SetActive(false);

        }

        if (lvlPlayed == false)
        {
            yield return new WaitForSeconds(1.0f);
            blurred.SetActive(true);
            ghostMode.SetActive(true);
            lvlPlayed = true;

            yield return new WaitForSeconds(3.0f);
            blurred.SetActive(false);
            ghostMode.SetActive(false);
            timerStart = true;
        }
    }
}
