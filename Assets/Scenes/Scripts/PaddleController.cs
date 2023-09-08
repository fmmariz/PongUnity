using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // Start is called before the first frame update
    private KeyCode upRight;
    private KeyCode downLeft;
    private bool isVertical;
    private float speed = 12f;
    public ScreenEdge edge;
    private bool autoplay = true;
    private float maxMoveableValue = -1;
    private float minMoveableValue = -1;

    private Bounds fieldBounds;

    int score = 0;

    void Start()
    {
        fieldBounds = GameController.instance.GetFieldBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float movement = 0;
        if(!autoplay){
            if(Input.GetKey(upRight)){
                movement +=1;
            }
            if(Input.GetKey(downLeft)){
                movement -=1;
            }
        }else
        {
            movement = AutoplayMovement(movement);
        }

        Vector3 finalMovement = new Vector3();
        if(isVertical){
            finalMovement.y = movement * Time.deltaTime * speed;
        }else{
            finalMovement.x = movement * Time.deltaTime * speed;
        }
        Vector3 newPosition = transform.position + finalMovement;
        if(isVertical){
            if(newPosition.y >= maxMoveableValue) newPosition.y = maxMoveableValue;
            if(newPosition.y <= minMoveableValue) newPosition.y = minMoveableValue;
        }else{
            if(newPosition.x >= maxMoveableValue) newPosition.x = maxMoveableValue;
            if(newPosition.x <= minMoveableValue) newPosition.x = minMoveableValue;
        }
        transform.position = newPosition;
    }

    private float AutoplayMovement(float movement)
    {
        BallController ball = GameController.instance.getCurrentBall();
        if (isVertical)
        {
            if (ball != null)
            {
                if (ball.transform.position.y > transform.position.y) movement += 1;
                if (ball.transform.position.y < transform.position.y) movement -= 1;
            }else{
                if (fieldBounds.center.y > transform.position.y) movement += 1;
                if (fieldBounds.center.y < transform.position.y) movement -= 1;
            }
        }
        else
        {
            if (ball != null)
            {
                if (ball.transform.position.x > transform.position.x) movement += 1;
                if (ball.transform.position.x < transform.position.x) movement -= 1;
            }else{
                if (fieldBounds.center.x > transform.position.x) movement += 1;
                if (fieldBounds.center.x < transform.position.x) movement -= 1;
            }
        }

        return movement;
    }

    public void SetUpRight(KeyCode keyCode){
        upRight = keyCode;
        autoplay = false;
    }
    
    public void SetDownLeft(KeyCode keyCode){
        downLeft = keyCode;
        autoplay = false;
    }

    public void SetEdge(ScreenEdge edge){
        this.edge = edge;
    }

    public ScreenEdge GetEdge(){
        return edge;
    }

    public void SetVertical(bool vertical){
        isVertical = vertical;
        Bounds fieldBounds = GameController.instance.GetFieldBoundaries();
        Bounds paddle = GetComponent<SpriteRenderer>().bounds;

        if(isVertical){
            float maxToTouch = Mathf.Abs(paddle.max.y - paddle.center.y);
            maxMoveableValue = fieldBounds.max.y - maxToTouch;
            minMoveableValue = fieldBounds.min.y + maxToTouch;
        }else{
            float maxToTouch = Mathf.Abs(paddle.max.x - paddle.center.x);
            maxMoveableValue = fieldBounds.max.x - maxToTouch;
            minMoveableValue = fieldBounds.min.x + maxToTouch;
        }
    }

    public bool IsVertical(){
        return isVertical;
    }

    public int GetScore(){
        return score;
    }

    public void AddScore(){
        score++;
    }

    public void RemoveScore(){
        score--;
    }
}
