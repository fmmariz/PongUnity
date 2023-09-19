using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public GameObject field;
    [SerializeField] GameObject scoreSpot;
    [SerializeField] public GameObject paddle;
    [SerializeField] public GameObject scoreBorder;
    [SerializeField] public GameObject topBorder;
    [SerializeField] public GameObject bottomBorder;

    [SerializeField] PaddleController paddleController;

    [SerializeField] InputAction up;
    [SerializeField] InputAction down;

    public bool autoplay;
    public bool upright = false;

    public int playerIndex;

    private int score = 0;

    public bool isVertical = false;

    float moveValues = 0;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame

    private void FixedUpdate(){
        if(autoplay == true)
        {
            if (GameController.instance.getCurrentBall() == null) return;
            AutoPlay();
        }
        if(playerIndex == 0){
            HandleInput(KeyCode.W,KeyCode.S);
        }
        if(playerIndex == 2){
            HandleInput(KeyCode.DownArrow, KeyCode.UpArrow); // Flipped 180 so strokes must be reversed
        }
        paddleController.Move(new Vector2(0,moveValues));
    }

    private void HandleInput(KeyCode up, KeyCode down){
        moveValues = 0;
        if (Input.GetKey(up))
        {
            if(autoplay) autoplay = false;
            moveValues = 1;
        }
        if(Input.GetKey(down)){
            if(autoplay) autoplay = false;
            moveValues = -1;
        }
    }

    private void AutoPlay()
    {
        float height;
        float ballHeight;
        switch (playerIndex)
        {
            case 0:
                height = paddle.transform.position.y;
                ballHeight = GameController.instance.getCurrentBall().transform.position.y;
                if (height > ballHeight) moveValues = -1;
                if (height < ballHeight) moveValues = 1;
                break;
            case 1:
                height = paddle.transform.position.x;
                ballHeight = GameController.instance.getCurrentBall().transform.position.x;
                if (height > ballHeight) moveValues = 1;
                if (height < ballHeight) moveValues = -1;
                break;
            case 2:
                height = paddle.transform.position.y;
                ballHeight = GameController.instance.getCurrentBall().transform.position.y;
                if (height > ballHeight){ 
                    moveValues = 1; 
                    Debug.Log("Hello!");
                    }
                if (height < ballHeight){ 
                    moveValues = -1; 
                    Debug.Log("Bye!");
                    }
                paddleController.Move(new Vector2(0,1));
                break;
            case 3:
                height = paddle.transform.position.x;
                ballHeight = GameController.instance.getCurrentBall().transform.position.x;
                if (height > ballHeight) moveValues = -1;
                if (height < ballHeight) moveValues = 1;
                break;
        }
    }

    public Bounds GetPlayerFieldBounds(){
        return field.GetComponent<SpriteRenderer>().bounds;
    }

    public GameObject getScoreSpot(){
        return scoreSpot;
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

    public void Move(Vector2 vector)
    {
        if(autoplay != true){
            moveValues = vector.y;
        }
    }

    private void OnDisable() {
        up.Disable();  
        down.Disable();    

    }

    private void OnEnable(){
        up.Enable();
        down.Enable();    
    }

    public int GetPlayerIndex(){
        return playerIndex;
    }

}
