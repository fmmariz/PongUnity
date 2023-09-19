using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputControl : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;
    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        var controllers = FindObjectsOfType<PlayerController>();
        var index = playerInput.playerIndex;    
        playerController = controllers.FirstOrDefault(m => m.playerIndex == index);

    }

    public void OnMove(CallbackContext ctx){
        if(playerController == null) return;
        playerController.Move(ctx.ReadValue<Vector2>());
    }
   
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
