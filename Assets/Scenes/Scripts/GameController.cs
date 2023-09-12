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
    [SerializeField] UIController uiController;
    private BallController ballInGame = null;

    private GameStates currentGameState = GameStates.NOT_STARTED;

    int playerCount = 2;
    List<PlayerController> playerControllers;
    Dictionary<PlayerController, GameObject> playerMeshDict;


    void Start()
    {
        playerControllers = new List<PlayerController>();
        spawnPlayers(playerCount);
        assignScores();
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKey(KeyCode.Space)){
            currentGameState = GameStates.ONGOING;
        }
        if(currentGameState == GameStates.ONGOING){
            if(ballInGame == null){
                ballInGame = SpawnBall();
            }

            if(Input.GetKey(KeyCode.R)){
                Destroy(ballInGame.gameObject);
            }
        }
    }

    void spawnPlayers(int number){
        if(number == 2){
            playerControllers.Add(spawnPlayer(true,0, "P1"));
            playerControllers.Add(spawnPlayer(false,180, "P2"));
        }else if(number == 4){
            int baseAngle = 45;
            int spinAngle = 90;
            playerControllers.Add(spawnPlayer(true, baseAngle, "P1"));
            playerControllers.Add(spawnPlayer(false, baseAngle + spinAngle, "P2"));
            playerControllers.Add(spawnPlayer(false, baseAngle + 2*spinAngle, "P3"));
            playerControllers.Add(spawnPlayer(false, baseAngle + 3*spinAngle, "P4"));
        }
    }

    void assignScores()
    {
        playerMeshDict = new Dictionary<PlayerController, GameObject>();
        foreach(PlayerController player in playerControllers)
        {
            playerMeshDict[player] = uiController.CreateNewScore(player);
        }
        UpdateScores();
    }

    public void ApplyScore(PlayerController scored, PlayerController scorer)
    {
        if(scored == scorer || scorer == null){
           if(playerControllers.Count == 4){
            scored.RemoveScore();
           }else{
            foreach(PlayerController pc in playerControllers){
                if(pc != scored){
                    Debug.Log("Added Score");
                    pc.AddScore();
                }
            }
           }
        }else{
            Debug.Log("Added Score");
            scorer.AddScore();
        }
        UpdateScores();
    }

    void UpdateScores(){
        int score;
        string currentScoreHolder = null;
        foreach(PlayerController pc in playerMeshDict.Keys)
        {
            score = pc.GetScore();
            if(score == 3){ currentScoreHolder = pc.playerName; }
            playerMeshDict[pc].GetComponent<TextMeshProUGUI>().text = score.ToString();
        }
        if(currentScoreHolder != null){
            currentGameState = GameStates.ENDED;
            ShowWinner(currentScoreHolder+" HAS WON!");
        }
    }

    void ShowWinner(string message){
        uiController.DisplayWinnerMessage(message);
    }
    
    PlayerController spawnPlayer(bool isPlayable, int angle, string name){
        float size;
        GameObject newPlayerField = Instantiate(playableFieldPrefab, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)));
        PlayerController controller = newPlayerField.GetComponent<PlayerController>();

        if(playerCount == 2){
            size = 50f;
            newPlayerField.transform.localScale = new Vector3(size, size,1f);
            newPlayerField.transform.rotation = Quaternion.Euler(new Vector3(0,0, angle));
        }else{
            size = 15f;
            newPlayerField.transform.localScale = new Vector3(size, size,1f);
            Bounds field = controller.GetPlayerFieldBounds();
            Vector3 topRight = field.max;
            Vector3 topLeft = new Vector3(field.min.x, topRight.y);
            newPlayerField.transform.rotation = Quaternion.Euler(new Vector3(0,0, angle));
            newPlayerField.transform.position = -newPlayerField.transform.right * (Vector3.Distance(topLeft,topRight)/2);
        }
        controller.playerName = name;
        return controller;
    }

    BallController SpawnBall(){
        if(ballInGame != null) return ballInGame;
        float balltilt = 0;
        if(playerCount == 4) balltilt = 45; 
        GameObject newBall = Instantiate(ballPrefab, getBallSpawningZone(), Quaternion.Euler(new Vector3(0,0,balltilt)));
        BallController ballController = newBall.GetComponent<BallController>();
        Quaternion directionQ = Quaternion.Euler(0f,0f, Random.Range(0,360));
        Vector3 directionV = directionQ * Vector3.up;
        newBall.GetComponent<Rigidbody2D>().velocity = directionV*5f;
        // ballController.SetDirection(directionV);
        
        // ballController.SetSpeed(5f);
        return ballController;
    }

    Vector3 getBallSpawningZone(){
        return new Vector3(0,0,-1f);
    }

    public BallController getCurrentBall(){
        return ballInGame;
    }

}
