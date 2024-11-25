using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{
    // Variables related to player movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2D;
    Vector2 move;
    public float speed = 3.0f;

    // Variables related to the health system
    public int maxHealth = 5;
    public int health { get{return currentHealth; }}
    int currentHealth = 1;

    // Start is called before the first frame update
    void Start()
    {
     MoveAction.Enable();
     rigidbody2D = GetComponent<Rigidbody2D>();
     //currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {    
         move = MoveAction.ReadValue<Vector2>();
    }

    //FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
       Vector2 position = (Vector2)transform.position + move * 6.0f * Time.deltaTime;
       rigidbody2D.MovePosition(position);
    }

    public void ChangeHealth (int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
