using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject attack, jump, move, goDark, backGround, warning, congrats, enemy;

    int playerSeen2;
    int loadLvl;
    public int times;
    float inputX;

    bool start;
    bool jumpText;
    bool attackText;
    bool warningText;
    bool congratsText;
    // Start is called before the first frame update
    void Start()
    {
        //playerSeen2 = PlayerPrefs.SetInt("PlayerSeen2", 0);
        start = true;
        loadLvl = PlayerPrefs.GetInt("Levels");
        StartCoroutine(Canvas());
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        
        if((inputX < 0 || inputX > 0) && times == 0)
        {
            move.SetActive(false);
            jumpText = true;
            StartCoroutine(Canvas());
            ++times;
        }

        if (Input.GetKey(KeyCode.Space) && times == 1)
        {
            attackText = true;
            jump.SetActive(false);
            StartCoroutine(Canvas());
            ++times;
        }

        if (Input.GetKey(KeyCode.Return) && times == 2)
        {
            warningText = true;
            attack.SetActive(false);
            StartCoroutine(Canvas());
            ++times;
        }

       if(enemy == null)
        {
            congrats.SetActive(true);

            StartCoroutine(Canvas());
        }
       
    }

    IEnumerator Canvas()
    {
        if (start)
        {
            yield return new WaitForSeconds(.2f);
            move.SetActive(true);
            start = false;
        }

        if (jumpText)
        {
            yield return new WaitForSeconds(1.0f);
            jump.SetActive(true);
            jumpText = false;
        }

        if (attackText)
        {
            yield return new WaitForSeconds(1.0f);
            attack.SetActive(true);
            attackText = false;
        }

        if (warningText)
        {
            yield return new WaitForSeconds(1.0f);
            backGround.SetActive(true);
            warning.SetActive(true);

            yield return new WaitForSeconds(3.0f);
            backGround.SetActive(false);
            warning.SetActive(false);
            warningText = false;

        }

        if(enemy == null)
        {
            yield return new WaitForSeconds(3.0f);
            PlayerPrefs.SetInt("Levels", ++loadLvl);
            SceneManager.LoadScene(loadLvl);
        }
    }
}
