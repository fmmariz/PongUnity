using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Vector3 directions;
    private float speed;
    private Rigidbody2D _rb;
    private float maxX;
    private float minX;
    private float maxY;
    private float minY;

    private float distanceToEdge;
    private Bounds obtainedFieldBounds;

    private Dictionary<ScreenEdge,PaddleController> paddle;
    private PaddleController currentOwner;

    void Start()
    {
        paddle = new Dictionary<ScreenEdge, PaddleController>();
        _rb = GetComponent<Rigidbody2D>();
        distanceToEdge = GetComponent<SpriteRenderer>().bounds.max.y - GetComponent<SpriteRenderer>().bounds.center.y;
        //obtainedFieldBounds = GameController.instance.GetFieldBoundaries();
        //foreach(PaddleController paddlePlayer in GameController.instance.getPlayers()){
         //   paddle[paddlePlayer.edge] = paddlePlayer;
        //}
    }


    void Update()
    {
        Vector3 pos = transform.position;
        var distance = Time.deltaTime * speed;

        Vector3 newPosition = transform.position + (distance * directions);

        if(newPosition.y + distanceToEdge > obtainedFieldBounds.max.y
            && !paddle.ContainsKey(ScreenEdge.TOP)){
            directions.y = -directions.y;
        }else if(newPosition.y - distanceToEdge < obtainedFieldBounds.min.y
            && !paddle.ContainsKey(ScreenEdge.BOTTOM)){
            directions.y = -directions.y;
        }else if(newPosition.x + distanceToEdge > obtainedFieldBounds.max.x
            && !paddle.ContainsKey(ScreenEdge.RIGHT)){
            directions.x = -directions.x;
        }else if(newPosition.x - distanceToEdge < obtainedFieldBounds.min.x
             && !paddle.ContainsKey(ScreenEdge.LEFT)){
            directions.x = -directions.x;
        }

        transform.position = newPosition;
    }



    public void SetDirection(UnityEngine.Vector2 direction){
        this.directions = direction;
    }

    public void FlipHorizontal(){
        this.directions.x = -this.directions.x;
        Debug.Log("Flipped horizontal");
    }
    public void FlipVertical(){
        this.directions.y = -this.directions.y;
        Debug.Log("Flipped vertical");
    }

    public void SetSpeed(float speed){
        this.speed = speed;
    }

    void OnCollisionEnter2D(Collision2D other) {
        PaddleController paddle = other.gameObject.GetComponent<PaddleController>();
            if(paddle != null){ //Crude hitting the paddle collision
            currentOwner = paddle;
                Bounds thisBounds = GetComponent<Collider2D>().bounds;
                Bounds targetBounds = other.collider.bounds;
                Vector3 normal;
                if(paddle.IsVertical()){
                    normal = new Vector3(1,0,0);
                }else{
                    normal = new Vector3(0,1,0);
                }
                directions = Vector3.Reflect(directions, normal);
            }
        }
}
