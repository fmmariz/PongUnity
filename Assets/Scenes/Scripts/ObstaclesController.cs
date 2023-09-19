using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    [SerializeField] 
    private GameObject _paddleObstacle;
    [SerializeField]
    private GameObject _circleObstacle;
    [SerializeField]
    private GameObject _squareObstacle;

    private List<GameObject> _obstaclesInPlay;

    private float _SCALEOFFSET = 10f;

    private void Awake(){
        _obstaclesInPlay = new List<GameObject>();
    }


    private GameObject SpawnObstacle(
        GameObject parent,
        GameObject prefab, Vector3 coordinates, float angle){
            GameObject newObstacle = Instantiate(
                prefab,
                coordinates,
                Quaternion.Euler(0,0,angle),
                parent.transform);
            newObstacle.transform.localScale = newObstacle.transform.localScale * _SCALEOFFSET;
            _obstaclesInPlay.Add(newObstacle);
            return newObstacle;
    }

    public GameObject SpawnRandomObstacle(
        GameObject parent, Vector3 coordinates, float angle, float scale){
        List<GameObject> prefabList = new List<GameObject>{
            _circleObstacle,
            _squareObstacle,
            _paddleObstacle};
        GameObject chosenPrefab =  prefabList[Random.Range(0, prefabList.Count)];

        GameObject newObstacle = Instantiate(
            chosenPrefab,
            coordinates,
            Quaternion.Euler(0,0,angle),
            parent.transform);
        newObstacle.transform.localScale = newObstacle.transform.localScale * _SCALEOFFSET * scale;
        _obstaclesInPlay.Add(newObstacle);
        return newObstacle;
    }

    public GameObject SpawnSquareObstacle(GameObject parent, Vector3 coordinates, float angle){
        return SpawnObstacle(parent, _squareObstacle, coordinates, angle);
    }
    public GameObject SpawnCircleObstacle(GameObject parent, Vector3 coordinates){
        return SpawnObstacle(parent, _circleObstacle, coordinates, 0);
    }
    public GameObject SpawnPaddleObstacle(GameObject parent, Vector3 coordinates, float angle){
        return SpawnObstacle(parent, _paddleObstacle, coordinates, angle);
    }



    public void RemoveAllObstacles(){
        foreach(GameObject obstacle in _obstaclesInPlay){
            RemoveObstacle(obstacle);
        }
    }

    public void RemoveObstacle(GameObject gameObject){
        if(_obstaclesInPlay.Contains(gameObject)){
            _obstaclesInPlay.Remove(gameObject);
            gameObject.transform.parent = null;
            Destroy(gameObject);
        }
    }

}
