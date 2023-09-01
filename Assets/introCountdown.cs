using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class introCountdown : MonoBehaviour
{
    public float _countDownTime;
    public int _countDownTimeRef;

    public Transform _countDownCanvas;
    public TMP_Text _countDown;

    public bool _isStarted;
    // Start is called before the first frame update
    void Start()
    {
        _countDownTime = _countDownTimeRef;
    }

    // Update is called once per frame
    void Update()
    {
        if (_countDownTime >= 0)
        {
            _countDownTime -= Time.deltaTime;
        }

        if (_countDownTime <= 0)
        {

        }
    }
}
