using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pauseBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public menuInputs _menuInputs;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void _restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void _mainMenu()
    {
        SceneManager.LoadScene("main menu");
    }
    public void _resume()
    {
        Time.timeScale = 1;
        _menuInputs._isPaused = false;
        _menuInputs._pausedMenu.transform.gameObject.SetActive(false);
    }
}
