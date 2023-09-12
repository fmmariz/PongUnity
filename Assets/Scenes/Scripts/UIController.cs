using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
        [SerializeField] GameObject scoreTextPrefab;
        [SerializeField] GameObject winnerMessage;


    public GameObject CreateNewScore(PlayerController playerController){
        GameObject scoreMarker = playerController.getScoreSpot();
        Vector2 position = Camera.main.WorldToScreenPoint(scoreMarker.transform.position);
        GameObject textMesh = Instantiate(scoreTextPrefab, position, Quaternion.Euler(new Vector3(0,0,0)));
        textMesh.transform.SetParent(this.gameObject.transform, true);
        return textMesh;
    }

    public GameObject DisplayWinnerMessage(string message){
        GameObject textMesh = Instantiate(winnerMessage, new Vector2(Screen.width/2,Screen.height/2), Quaternion.Euler(new Vector3(0,0,0)));
        textMesh.transform.SetParent(this.gameObject.transform, true);
        return textMesh;
    }
}
