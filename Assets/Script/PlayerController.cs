using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{
    public InputAction MoveAction;
    Rigidbody2D rigidbody2D;
    Vector2 move;                 
    // Start is called before the first frame update
    void Start()
    {
     MoveAction.Enable();
     rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {    
         move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
    }

    void FixedUpdate()
    {
       Vector2 position = (Vector2)transform.position + move * 6.0f * Time.deltaTime;
       rigidbody2D.MovePosition(position);
    }
}
