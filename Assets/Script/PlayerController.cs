using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;
    private Rigidbody2D rigidbody2D;
    Vector2 move;
    public int maxHealth = 5;
    int currentHealth;

    public int health 
    {
        get 
        {
            return currentHealth;
        }
    }


    public float timeInvincible = 7.0f;
    bool isInvincible;
    float damageCooldown;

    // Variables for Player Animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    public InputAction projectileShoot;

    public InputAction talkAction;

    // Variables related to audio
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        moveAction.Enable();
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = 1;
        animator = GetComponent<Animator>(); 
        projectileShoot.Enable();
        ChangeHealth(1);
        talkAction.Enable();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
        //Debug.Log(move);

        if(isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if(damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(projectileShoot.triggered)
        {
            Launch();
        }

        if(talkAction.triggered)
        {
            FindFriend();
        }

    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2D.position + move * 3.0f * Time.deltaTime;
       rigidbody2D.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {   
            if(isInvincible)
                return;
            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);


        Debug.Log(currentHealth + "/" + maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Launch");
    }

    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if(hit.collider != null)
        {
            Debug.Log("Raycast touched " + hit.collider.gameObject.name);
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

            if(character != null)
            {
                UIHandler.instance.DisplayDialogue(character.shams);
            }

        }
    }
        public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }   
}