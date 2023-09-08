using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class GameController : MonoBehaviour
{
    
    public static GameController instance {get; private set;}

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }
    
    public GameObject paddlePrefab;
    public GameObject ballPrefab;
    public GameObject field;
    [SerializeField] public List<TextMeshProUGUI> textMeshList; //0-LEFT 1-BOTTOM 2-RIGHT 3-TOP
    Dictionary<ScreenEdge, TextMeshProUGUI> edgeScore;
    private BallController ballInGame = null;

    private float paddleOffset = 0.25f;

    private List<PaddleController> players;

    void Start()
    {
         edgeScore = new Dictionary<ScreenEdge, TextMeshProUGUI>(){
            {ScreenEdge.LEFT, textMeshList[0]},
            {ScreenEdge.BOTTOM, textMeshList[1]},
            {ScreenEdge.RIGHT, textMeshList[2]},
            {ScreenEdge.TOP, textMeshList[3]},
        };
        players = new List<PaddleController>();
        players.Add(spawnPaddle(ScreenEdge.RIGHT,KeyCode.UpArrow, KeyCode.DownArrow, false));
        players.Add(spawnPaddle(ScreenEdge.LEFT,KeyCode.W, KeyCode.S, true));
        //players.Add(spawnPaddle(ScreenEdge.TOP,KeyCode.UpArrow, KeyCode.DownArrow, false));
        //players.Add(spawnPaddle(ScreenEdge.BOTTOM,KeyCode.W, KeyCode.S, false));
        ballInGame = spawnBall();
        setupScores();
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void setupScores(){
        foreach(TextMeshProUGUI textMesh in textMeshList){
            textMesh.gameObject.SetActive(false);
        }
        foreach(PaddleController paddle in players){
            edgeScore[paddle.edge].gameObject.SetActive(true);
        }
    }

    public void UpdateScore(ScreenEdge scoredEdge, PaddleController shotOwner){
        if(players.Count == 2){
            players.Find(player => {return player.edge != scoredEdge;}).AddScore();
        }else{
            PaddleController hitPlayer = players.Find(player => {return player.edge == scoredEdge;});
            if(hitPlayer == shotOwner)
            {
                hitPlayer.RemoveScore();
            }else{
                players.Find(player => {return player.edge == shotOwner.edge;}).AddScore();
            }
        }

        foreach(PaddleController paddle in players){
            edgeScore[paddle.edge].text = paddle.GetScore().ToString();
        }
    }

    PaddleController spawnPaddle(ScreenEdge edge, KeyCode upRight, KeyCode downLeft, bool isPlayable){
        var spriteRenderer = field.GetComponent<SpriteRenderer>();
        Vector3 center = spriteRenderer.bounds.center;
        Vector3 bottomLeft = spriteRenderer.bounds.min;
        Vector3 topRight = spriteRenderer.bounds.max;
        Vector3 spawnCoords = new Vector3();
        float angle = 0;
        switch(edge){
            case ScreenEdge.LEFT:
                spawnCoords = new Vector3(bottomLeft.x+paddleOffset, center.y, -0.5f);
                upRight = KeyCode.W;
                downLeft = KeyCode.S;
            break;
            case ScreenEdge.BOTTOM:
                spawnCoords = new Vector3(center.x, bottomLeft.y+paddleOffset, -0.5f);
                upRight = KeyCode.C;
                downLeft = KeyCode.V;
                angle = 90;
            break;
            case ScreenEdge.RIGHT:
                spawnCoords = new Vector3(topRight.x-paddleOffset, center.y, -0.5f);
                upRight = KeyCode.UpArrow;
                downLeft = KeyCode.DownArrow;
            break;
            case ScreenEdge.TOP:
                spawnCoords = new Vector3(center.x, topRight.y-paddleOffset, -0.5f);
                upRight = KeyCode.KeypadMinus;
                downLeft = KeyCode.Keypad9;
                angle = 90;
            break;
        }

        GameObject newPaddle = Instantiate(paddlePrefab, spawnCoords, Quaternion.identity);
        newPaddle.transform.rotation = Quaternion.Euler(0,0, angle);
        PaddleController paddleController = newPaddle.GetComponent<PaddleController>();
        if(isPlayable){
            paddleController.SetDownLeft(downLeft);
            paddleController.SetUpRight(upRight);
        }
        paddleController.SetEdge(edge);
        paddleController.SetVertical( edge != ScreenEdge.LEFT && edge != ScreenEdge.RIGHT ? false : true);
        return paddleController;
    }

    BallController spawnBall(){
        if(ballInGame != null) return ballInGame;
        GameObject newBall = Instantiate(ballPrefab, getBallSpawningZone(), Quaternion.identity);
        BallController ballController = newBall.GetComponent<BallController>();
        ballController.SetDirection((new Vector2(Random.Range(0,2)*2-1,Random.Range(-1,1))).normalized);
        ballController.SetSpeed(6f);
        return ballController;
    }

    Vector3 getBallSpawningZone(){
        float spawnRadius = 2f;
        if(field == null) return new Vector3(0,0,0);
        var spriteRenderer = field.GetComponent<SpriteRenderer>();
        var bounds = spriteRenderer.bounds;
        double centerx = bounds.center.x;
        double centery = bounds.center.y;
        if(players.Count == 2){
        double minHeight = bounds.min.y;
        double maxHeight = bounds.max.y;
         return new Vector3(
            (float) centerx,
            (float) Random.Range(
                (float) minHeight,
                (float) maxHeight
                )
            ,-0.5f);
        }else{
            return new Vector3(
            (float) centerx+ Random.Range(-spawnRadius, spawnRadius),
            (float) centery+ Random.Range(-spawnRadius, spawnRadius),
            -0.5f);
        }
    }

    public Bounds GetFieldBoundaries(){
        return field.GetComponent<SpriteRenderer>().bounds;
    }

    public List<PaddleController> getPlayers(){
        return players;
    }

    public BallController getCurrentBall(){
        return ballInGame;
    }

}
