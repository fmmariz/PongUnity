using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStopObstacleController : MonoBehaviour
{
    private Rigidbody2D _body;
    private Vector3 _newPosition;
    private Vector3 _oldPosition;
    private float _wait;
    private float _move;
    private float _waitTimeElapsed;
    private readonly float _moveSpeed = 1f;
    private float _distanceMoved;
    private float _startTime;
    private float _moveTimeElapsed;
    private bool _isMoving;
    private bool _moveToNew = false;

    public void SetMovementPattern(Vector3 targetSpot, float waitTimeInMs, float timeToReach){
        _newPosition = targetSpot;
        _wait = waitTimeInMs;
        _move = timeToReach;

        _distanceMoved = Vector3.Distance(targetSpot, _oldPosition);
    }
    
    void Awake(){
        _body = GetComponent<Rigidbody2D>();
        _waitTimeElapsed = 0f;
        _oldPosition = transform.position;
    }

    void FixedUpdate()
    {
        _waitTimeElapsed += Time.fixedDeltaTime;
        if(_waitTimeElapsed >= _wait)
        {
            
            if(!_isMoving){
                _startTime = Time.fixedDeltaTime;
                _moveTimeElapsed = 0f;
                _isMoving = true;
                _moveToNew = !_moveToNew;
            }
            if(_moveToNew){
                MoveToPosition(_newPosition);
            }else{
                MoveToPosition(_oldPosition);
            }
            if(_waitTimeElapsed >= _wait + _move){
                _waitTimeElapsed = 0;
                _isMoving = false;
            }
        }
    }


    private void MoveToPosition(Vector3 position){
        _moveTimeElapsed += Time.fixedDeltaTime;
        float _percentageMoved = _moveTimeElapsed / _move;

        Vector3 newPos = Vector3.Lerp(transform.position, position, _percentageMoved);

        _body.MovePosition(newPos);
    }
}
