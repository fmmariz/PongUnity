using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private UnityEngine.Vector3 directions;
    private float speed;
    private Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Time.deltaTime * 3f;
        _rb.MovePosition((UnityEngine.Vector2) (transform.position + (directions * distance)));
    }

    public void SetDirection(UnityEngine.Vector2 direction){
        this.directions = direction;
    }

    public void SetSpeed(float speed){
        this.speed = speed;
    }
}
