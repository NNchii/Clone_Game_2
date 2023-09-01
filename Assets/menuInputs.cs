using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class menuInputs : MonoBehaviour
{
    public bool _isPaused;
    public Transform _pausedMenu;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused == false)
            {
                _isPaused = true;
                _pausedMenu.transform.gameObject.SetActive(true);
                Time.timeScale = 0;
            }else if (_isPaused == true)
            {
                _isPaused = false;
                _pausedMenu.transform.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
            
        }
    }
    
}
