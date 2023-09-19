using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField]
    private GameObject _freezePrefab;
    [SerializeField]
    private GameObject _frenzyPrefab;

    private List<GameObject> _powerUpsInGame;
    private List<GameObject> _powerUpPrefabs;

    void Start()
    {
        _powerUpsInGame = new List<GameObject>();
        _powerUpPrefabs = new List<GameObject>{_frenzyPrefab, _freezePrefab};
    }

    public GameObject SpawnPowerUp(GameObject parent, Vector3 coordinate){
        if(_powerUpsInGame.Count >= 2) return null;
        GameObject chosenPrefab = _powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Count)];
        Debug.Log("Spawned a "+chosenPrefab.name+".");
            GameObject newPowerUp = Instantiate(
                chosenPrefab,
                coordinate,
                Quaternion.Euler(0,0,0),
                parent.transform);
            newPowerUp.transform.localScale = newPowerUp.transform.localScale;
            newPowerUp.transform.parent = parent.transform;
            _powerUpsInGame.Add(newPowerUp);
            return newPowerUp;
    }

    public void RemoveAllObjects(){
        foreach(GameObject powerUp in _powerUpsInGame){
            RemoveObject(powerUp);
        }
    }

    public void RemoveObject(GameObject gameObject){
        _powerUpsInGame.Remove(gameObject);
        gameObject.transform.parent = null;
        Destroy(gameObject);
    }

    
}
