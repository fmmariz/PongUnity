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
    
    [SerializeField] GameObject playableFieldPrefab;
    [SerializeField] GameObject ballPrefab;
    Dictionary<ScreenEdge, TextMeshProUGUI> edgeScore;
    private BallController ballInGame = null;

    private float paddleOffset = 0.25f;

    int playerCount = 4;

    void Start()
    {
        setupScores();
        spawnPlayers(playerCount);
        spawnBall();
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void setupScores(){
        // foreach(TextMeshProUGUI textMesh in textMeshList){
        //     textMesh.gameObject.SetActive(false);
        // }
        // foreach(PaddleController paddle in players){
        //     edgeScore[paddle.edge].gameObject.SetActive(true);
        // }
    }

    public void UpdateScore(ScreenEdge scoredEdge, PaddleController shotOwner){
        // if(players.Count == 2){
        //     players.Find(player => {return player.edge != scoredEdge;}).AddScore();
        // }else{
        //     PaddleController hitPlayer = players.Find(player => {return player.edge == scoredEdge;});
        //     if(hitPlayer == shotOwner)
        //     {
        //         hitPlayer.RemoveScore();
        //     }else{
        //         players.Find(player => {return player.edge == shotOwner.edge;}).AddScore();
        //     }
        // }

        // foreach(PaddleController paddle in players){
        //     edgeScore[paddle.edge].text = paddle.GetScore().ToString();
        // }
    }

    void spawnPlayers(int number){
        if(number == 2){
            spawnPlayer(true,0);
            spawnPlayer(false,180);
        }else if(number == 4){
            int baseAngle = 45;
            int spinAngle = 90;
            spawnPlayer(true, baseAngle);
            spawnPlayer(false, baseAngle + spinAngle);
            spawnPlayer(false, baseAngle + 2*spinAngle);
            spawnPlayer(false, baseAngle + 3*spinAngle);
        }
    }

    void spawnPlayer(bool isPlayable, int angle){
        float size;
        GameObject newPlayerField = Instantiate(playableFieldPrefab, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)));
        if(playerCount == 2){
            size = 50f;
            newPlayerField.transform.localScale = new Vector3(size, size,1f);
            newPlayerField.transform.rotation = Quaternion.Euler(new Vector3(0,0, angle));
        }else{
            size = 15f;
            newPlayerField.transform.localScale = new Vector3(size, size,1f);
            PlayerController controller = newPlayerField.GetComponent<PlayerController>();
            Bounds field = controller.GetPlayerFieldBounds();
            Vector3 topRight = field.max;
            Vector3 topLeft = new Vector3(field.min.x, topRight.y);
            newPlayerField.transform.rotation = Quaternion.Euler(new Vector3(0,0, angle));
            newPlayerField.transform.position = -newPlayerField.transform.right * (Vector3.Distance(topLeft,topRight)/2);
        } 
    }

    BallController spawnBall(){
        if(ballInGame != null) return ballInGame;
        Debug.Log("Aie!");
        GameObject newBall = Instantiate(ballPrefab, getBallSpawningZone(), Quaternion.identity);
        BallController ballController = newBall.GetComponent<BallController>();
        ballController.SetDirection(new Vector2(Random.Range(0,2)*2-1,Random.Range(-1,1)).normalized);
        ballController.SetSpeed(6f);
        return ballController;
    }

    Vector3 getBallSpawningZone(){
        return new Vector3(0,0,-1f);
    }

    public BallController getCurrentBall(){
        return ballInGame;
    }

}
