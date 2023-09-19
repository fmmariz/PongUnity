using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingMovementController : MonoBehaviour
{
    Rigidbody2D _body;
    Vector3 _spawnPosition;
    Vector3 _oscillatingDistance;
    float _oscillatingSpeed;

    public void SetOscillation(float oscillatingSpeed, Vector3 oscilattingDistance){
        _oscillatingDistance = oscilattingDistance;
        _oscillatingSpeed = oscillatingSpeed;
    }
    
    void Awake(){
        _body = GetComponent<Rigidbody2D>();
        _spawnPosition = transform.position;
    }

    void FixedUpdate()
    {
        _body.MovePosition(_spawnPosition+ Mathf.Sin(Time.time) * Time.deltaTime * _oscillatingDistance);
    }
}
