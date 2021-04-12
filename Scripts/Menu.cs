using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    int loadLvl;

    void Start()
    {
        PlayerPrefs.SetInt("Levels", 0);

    }
   public void Play()
    {
        loadLvl = PlayerPrefs.GetInt("Levels");
        PlayerPrefs.SetInt("Levels", ++loadLvl);
        SceneManager.LoadScene(loadLvl);
    }
}
