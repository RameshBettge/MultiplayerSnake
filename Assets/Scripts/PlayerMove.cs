using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    bool inGame = false;

    Vector2 inputVector;
    Vector2 moveVector;

    float inputSqrMagnitude;
    float inputDeadzone = 0.2f;

    private void Update()
    {
        //if (!inGame && inputVector.sqrMagnitude)
        //{

        //}   

        GetInput();

    }

    private void GetInput()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        inputSqrMagnitude = inputVector.sqrMagnitude;

        //moveVector 
    }
}
