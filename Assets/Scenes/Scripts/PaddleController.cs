using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private float paddleVel = 5f;
    private Vector2 moveValue;

    private void FixedUpdate(){
        Vector3 tilted = (Vector3)moveValue;
        if(collidingWithTopwall && tilted.y >=0){
            tilted.y = 0;
        }
        if(collidingWithBotwall && tilted.y <=0){
            tilted.y = 0;
        }
        tilted = paddleVel * Time.fixedDeltaTime * tilted;
        tilted = Quaternion.Euler(0,0,transform.eulerAngles.z)*tilted;
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + (Vector2)tilted);
    }

    public void Move(Vector2 command){
        moveValue = new Vector2(0, command.y);
    }

    private bool collidingWithTopwall = false;
    private bool collidingWithBotwall = false;

        void OnCollisionEnter2D(Collision2D other) {
         if(other.gameObject.name == "topBorder"){
            collidingWithTopwall = true;
         }else if(other.gameObject.name == "bottomBorder")
         {
            collidingWithBotwall = true;
         }
    }

    private void OnCollisionExit2D(Collision2D other) {
         if(other.gameObject.name == "topBorder"){
            collidingWithTopwall = false;
         }else if(other.gameObject.name == "bottomBorder")
         {
            collidingWithBotwall = false;
         }
    }
}
