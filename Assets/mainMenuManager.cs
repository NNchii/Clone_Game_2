using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuManager : MonoBehaviour
{
    public void _play()
    {
        SceneManager.LoadScene("Scene 01");
    }
    public void _credits()
    {
        SceneManager.LoadScene("credits");
    }
    public void _exit()
    {
        Application.Quit();
    }

}
