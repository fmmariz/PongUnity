
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{

    private Rigidbody2D _rb;
    private PlayerController currentOwner;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("wall")
            || other.gameObject.CompareTag("paddle")
            || other.gameObject.CompareTag("obstacle"))
        {
            ContactPoint2D hit = other.GetContact(0);
            _rb.velocity = Vector2.Reflect(_rb.velocity, hit.normal);
            if(other.gameObject.CompareTag("paddle")){
                currentOwner = other.gameObject.transform.parent.parent.GetComponent<PlayerController>();
            }
        }else if(other.gameObject.GetComponent<ScoreController>()){
            PlayerController hit = other.gameObject.GetComponent<ScoreController>().PlayerController;
            ApplyScore(hit);
            Destroy(gameObject);
        }
    }

    void ApplyScore(PlayerController hit){
        GameController.instance.ApplyScore(hit,currentOwner);
    }
}
