using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _minVelocityToApply;
    [SerializeField] private float _maxVelocityToApply;

    private Vector3 _clickPoint;
    private bool _isDragging = false;
    private bool _isInputActive = true;

    [SerializeField] private Ball _selectedBall; //ref vers le SpawnManager pour connaitre la boule active 
    static public event Action OnBallShooted;

    // Start is called before the first frame update
    void Start()
    {
        _isDragging = false;
        _clickPoint =  Vector3.zero;
        _isInputActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isInputActive) { 
            return;
        }

        if (!_isDragging && Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _clickPoint = Input.mousePosition;
        }

        if(_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            var mouseDiff = _clickPoint - Input.mousePosition;
            var velocityToApply = new Vector3(mouseDiff.x, 0, mouseDiff.y);
            var velocityToApplyMagnitudeCoeff = Mathf.InverseLerp(0, Screen.height * 0.5f , velocityToApply.magnitude);
            velocityToApply = velocityToApply.normalized * (Mathf.Clamp(_maxVelocityToApply * velocityToApplyMagnitudeCoeff, _minVelocityToApply, _maxVelocityToApply));
            Debug.Log($"[Velocity to apply] X: {velocityToApply.x} Z : {velocityToApply.z}");
            //getBallComponent  
            if (PlayGroundManager.Instance.ActiveBall != null)
            {
                PlayGroundManager.Instance.ActiveBall.Velocity = velocityToApply;
                EnablePlayerInputs(false);
                OnBallShooted?.Invoke();
            }
        }
    }

    public void EnablePlayerInputs(bool enable)
    {
        _isInputActive = enable;
    }
}
