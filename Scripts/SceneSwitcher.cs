using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchScene()
    {
        SceneManager.LoadScene("Lvl2");
    }
}
