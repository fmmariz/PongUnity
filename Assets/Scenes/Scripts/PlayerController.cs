using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject field;
    [SerializeField] ScoreController scoreBar;
    [SerializeField] GameObject scoreSpot;

    public string playerName;

    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
