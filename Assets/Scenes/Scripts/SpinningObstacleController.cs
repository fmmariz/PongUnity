using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObstacleController : MonoBehaviour
{
    private Rigidbody2D _body;
    private float _rotationSpeed;
    private float _rotationSummation;
    public void SetRotationSpeed(float rotationSpeed){
        _rotationSpeed = rotationSpeed;
    }
    
    void Awake(){
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rotationSummation += _rotationSpeed;
        _body.MoveRotation(_rotationSummation);
        if(_rotationSummation>=360) _rotationSummation = 0;
    }
}
