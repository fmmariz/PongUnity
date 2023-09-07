using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject field;
    private GameObject ballInGame = null;

    void Start()
    {
        spawnBall();
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void spawnBall(){
        if(ballInGame != null) return;
        GameObject newBall = Instantiate(ballPrefab, getBallSpawningZone(), Quaternion.identity);
        BallController ballController = newBall.GetComponent<BallController>();
        ballController.SetDirection(new UnityEngine.Vector2(0,0));
        ballController.SetSpeed(0.5f);
        ballInGame = newBall;
    }

    Vector3 getBallSpawningZone(){
        if(field == null) return new Vector3(0,0,0);
        var spriteRenderer = field.GetComponent<SpriteRenderer>();
        var bounds = spriteRenderer.bounds;
        double center = bounds.center.x;
        double minHeight = bounds.min.y;
        double maxHeight = bounds.max.y;
        return new Vector3(
            (float) center,
            (float) Random.Range(
                (float) minHeight,
                (float) maxHeight
                )
            ,-0.5f);

    }
}
