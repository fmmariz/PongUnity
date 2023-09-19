using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using TMPro.EditorUtilities;
using UnityEngine.UIElements;


public class GameController : MonoBehaviour
{
    #region Singleton
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
    #endregion
    
    [SerializeField] GameObject playableFieldPrefab;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] UIController uiController;
    private BallController ballInGame = null;

    private GameStates currentGameState = GameStates.NOT_STARTED;

    List<PlayerController> playerControllers;
    Dictionary<PlayerController, GameObject> playerMeshDict;
    private float _distanceToEdges = 1f;
    private List<Bounds> _fieldBounds;

    private float _powerUpTimer = 3f;
    private float _powerUpTimerElapsed = 0f;

    private ObstaclesController _obstacleController;
    private PowerUpController _powerUpController;



    // CUSTOMIZABLE

    #region Modifiable Info (add to the UI eventually)
    int playerCount = 2;
    float widthScalingPlayer = 1f;
    float heightScalingPlayer = 1f;
    #endregion


    void Start()
    {
        _obstacleController = GetComponent<ObstaclesController>();
        _powerUpController = GetComponent<PowerUpController>();
        playerControllers = new List<PlayerController>();
        _fieldBounds = new List<Bounds>();
        SpawnPlayers(playerCount, widthScalingPlayer, heightScalingPlayer);
        CameraRecentering(playerCount == 4);
        AssignScores();
        SetupPlayingField();
        SpawnObstacles();
    }

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

        _powerUpTimerElapsed += Time.deltaTime;
        if(_powerUpTimerElapsed >= _powerUpTimer){
            Debug.Log("Put a powerup");
            SpawnPowerUp();
            _powerUpTimerElapsed = 0f;
        }
    }

    #region Scoring and Display Winner
    void AssignScores()
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
            if(score == 3){ currentScoreHolder = "P"+(pc.GetPlayerIndex()+1).ToString(); }
            playerMeshDict[pc].GetComponent<TextMeshProUGUI>().text = (pc.GetPlayerIndex()+1).ToString()+":"+score.ToString();
        }
        if(currentScoreHolder != null){
            currentGameState = GameStates.ENDED;
            ShowWinner(currentScoreHolder+" HAS WON!");
        }
    }

    void ShowWinner(string message){
        uiController.DisplayWinnerMessage(message);
    }

    #endregion 



    #region Spawning
    void SpawnPlayers(int number,float wScale, float hScale){
        if(number == 2){
            playerControllers.Add(spawnPlayer(true,0, 0, wScale, hScale));
            playerControllers.Add(spawnPlayer(false,180, 2, wScale, hScale));
        }else if(number == 4){
            int spinAngle = 90;
            playerControllers.Add(spawnPlayer(true, 0, 0, wScale, hScale));
            playerControllers.Add(spawnPlayer(false, spinAngle, 1, wScale, hScale));
            playerControllers.Add(spawnPlayer(false, 2*spinAngle, 2, wScale, hScale));
            playerControllers.Add(spawnPlayer(false, 3*spinAngle, 3, wScale, hScale));
        }
    }
    PlayerController spawnPlayer(bool isPlayable, int angle, int name, float wScale, float hScale)
    {
        float size;
        GameObject newPlayerField = Instantiate(playableFieldPrefab, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        PlayerController controller = newPlayerField.GetComponent<PlayerController>();
        controller.autoplay = !isPlayable;
        if (playerCount == 2)
        {
            size = 50f;
            newPlayerField.transform.localScale = new Vector3(size, size, 1f);
            FieldSizeModification(wScale, hScale, newPlayerField);
            newPlayerField.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            size = 15f;
            newPlayerField.transform.localScale = new Vector3(size, size, 1f);
            FieldSizeModification(wScale, hScale, newPlayerField);
            Bounds field = controller.GetPlayerFieldBounds();
            Vector3 bottomLeft = field.min;
            Vector3 topLeft = new(field.min.x, field.max.y);
            newPlayerField.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            newPlayerField.transform.position += -newPlayerField.transform.right * (Vector3.Distance(bottomLeft, topLeft) / 2);
        }

        newPlayerField.transform.parent = this.gameObject.transform;
        controller.playerIndex = name;
        return controller;
    }

    BallController SpawnBall(){
        if(ballInGame != null) return ballInGame;
        float balltilt = 0; 
        GameObject newBall = Instantiate(ballPrefab, getBallSpawningZone(), Quaternion.Euler(new Vector3(0,0,balltilt)));
        BallController ballController = newBall.GetComponent<BallController>();
        Quaternion directionQ = Quaternion.Euler(0f,0f, Random.Range(0,360));
        Vector3 directionV = directionQ * Vector3.up;
        newBall.GetComponent<Rigidbody2D>().velocity = directionV*5f;
        newBall.transform.SetParent(this.transform);
        newBall.transform.rotation = newBall.transform.parent.rotation;
        return ballController;
    }

    void SpawnObstacles(){
        GameObject _paddleObstacle = _obstacleController.SpawnPaddleObstacle(gameObject, new Vector3(3, 3, 0), 0);
        _paddleObstacle.AddComponent<SpinningObstacleController>();
        _paddleObstacle.GetComponent<SpinningObstacleController>().SetRotationSpeed(3f);

        GameObject _squareObstacle = _obstacleController.SpawnSquareObstacle(gameObject, new Vector3(3, -3, 0), 0);
        _squareObstacle.AddComponent<MovingStopObstacleController>();
        _squareObstacle.GetComponent<MovingStopObstacleController>()
        .SetMovementPattern(
            _squareObstacle.transform.position + new Vector3(-6,6,0), 3f, 3f);

        GameObject _circleObstacle = _obstacleController.SpawnCircleObstacle(gameObject, new Vector3(-3,-3, 0));
        _circleObstacle.AddComponent<OscillatingMovementController>();
        _circleObstacle.GetComponent<OscillatingMovementController>().SetOscillation(3f, new Vector3(100,100,0));
    }

    void SpawnPowerUp(){
        _powerUpController.SpawnPowerUp(this.gameObject, GetRandomPointInField(0.2f));
    }

    #endregion

    #region Field Modification
    private static void FieldSizeModification(float wScale, float hScale, GameObject newPlayerField)
    {

        PlayerController controller = newPlayerField.GetComponent<PlayerController>();

        //Some elements must not scale, so a countervalue must be added
        float maintainH = 1 / hScale;
        float maintainW = 1 / wScale;

        Vector3 fieldChange = new()
        {
            x = newPlayerField.transform.localScale.x,
            y = newPlayerField.transform.localScale.y
        };

        fieldChange.x *= wScale;
        fieldChange.y *= hScale;
        fieldChange -= newPlayerField.transform.localScale;
        fieldChange.z = 0;
        newPlayerField.transform.localScale += fieldChange;

        RescaleElement(controller.paddle, maintainH, maintainW);
        RescaleElement(controller.topBorder,maintainH);
        RescaleElement(controller.bottomBorder, maintainH);
        RescaleElement(controller.scoreBorder, maintainW); 
    }

    private static void RescaleElement(GameObject gameObject, float maintainH = 1f, float maintainW = 1f)
    {
        Vector3 originalSize = gameObject.transform.localScale;
        Vector3 modifiedSize = originalSize;
        modifiedSize.x *= maintainW;
        modifiedSize.y *= maintainH;
        modifiedSize.z = 0;
        gameObject.transform.localScale = modifiedSize;
    }

    private void SetupPlayingField(){
        foreach(PlayerController pc in playerMeshDict.Keys)
        {
            _fieldBounds.Add(pc.field.GetComponent<SpriteRenderer>().bounds);
        }

        if(playerCount == 4){
            Bounds newBound = new Bounds
            {
                min = new Vector3(_fieldBounds[0].max.x, _fieldBounds[0].min.y),
                max = new Vector3(_fieldBounds[1].max.x, _fieldBounds[1].min.y)
            };
            _fieldBounds.Add(newBound);
        }
    }

    private Vector3 GetRandomPointInField(float pctFromEdges){
        if(_fieldBounds.Count == 0) return new Vector3(0,0,0);
        Bounds chosenPrefab = _fieldBounds[Random.Range(0, _fieldBounds.Count)];
        float allowedRadius = (chosenPrefab.min.x + chosenPrefab.max.x)/2f*(1f-pctFromEdges);
        Vector3 center = chosenPrefab.center;
        Vector2 randomWithinCircle = Random.insideUnitCircle;
        Vector3 randomPos = new Vector3(randomWithinCircle.x, randomWithinCircle.y, 0);
        Debug.Log(randomPos);
        Debug.Log(allowedRadius);
        Vector3 chosenPoint = center + randomPos * allowedRadius;
        return chosenPoint;
    }

    #endregion

    #region Camera Work
    private void CameraRecentering(bool tilt)
    {
        if(tilt) Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(0,0,45);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        var (center, size) = CalculateOrthoSize();
        Camera.main.transform.position = center;
        Camera.main.orthographicSize = size;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        var bounds = new Bounds();
        foreach(Transform child in transform){
            var field = child.gameObject.GetComponent<PlayerController>().field;
            bounds.Encapsulate(field.GetComponent<SpriteRenderer>().bounds);
        }
        bounds.Expand(_distanceToEdges);
        float vertical = bounds.size.y;
        float horizontal = bounds.size.x * Camera.main.pixelHeight / Camera.main.pixelWidth;
        float size = Mathf.Max(horizontal,vertical)*0.5f;
        var center = bounds.center + new Vector3(0,0,-10);
        return (center,size);
    }
    
    #endregion

    #region Getters and Setters
    Vector3 getBallSpawningZone(){
        return new Vector3(0,0,-1f);
    }

    public BallController getCurrentBall(){
        return ballInGame;
    }

    #endregion
}
