using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class StatusEffects : MonoBehaviour
{

    public enum StatusTag{
        FREEZE,
        FRENZY
    }
    private float _duration;
    private float _speedModifier;

    private Color _statusColor;

    private float _timeElapsed;
    private PlayerController _afflictedPlayer;
    // Update is called once per frame
    void Update()
    {
        _afflictedPlayer.RefreshStatuses();
        _timeElapsed += Time.deltaTime;
        if(_timeElapsed>=_duration){
            _afflictedPlayer?.RemoveStatus(this);
            _afflictedPlayer?.RefreshStatuses();
            Destroy(this);
        }
    }

    public void SetAfflictedPlayer(PlayerController pController){
        _afflictedPlayer = pController;
    }

    public void SetDuration(float time){
        _duration = time;
    }

    public void RefreshDuration(){
        _timeElapsed = 0f;
    }

    public void SetSpeedModifier(float speedModifier){
        _speedModifier = speedModifier;
    }

    public float GetSpeedModifier(){
        return _speedModifier;
    }

    public void SetColor(Color statusColor){
        _statusColor = statusColor;
    }

    public Color GetColor(){
        return _statusColor;
    }
}
