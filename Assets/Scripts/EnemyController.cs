using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private float speed = 3f;
    public bool vertical;
    public ParticleSystem smokeEffect;
    Rigidbody2D rigidbody2d;
    Animator animator;
    int direction = 2;

    public static int amount = 0;
    public bool broken  = false;

    AudioSource audioSource;
    public AudioClip robotFixed;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (broken)
        {
            
            return;
        }
        if (direction == 1)
        {
            animator.SetFloat("MoveY", 0);
            animator.SetFloat("MoveX", -1);
        }
        else if (direction == 2)
        {
            animator.SetFloat("MoveY", 0);
            animator.SetFloat("MoveX", 1);
        }
        else if (direction == 3)
        {
            animator.SetFloat("MoveY", 1);
            animator.SetFloat("MoveX", 0);
        }
        else
        {
            animator.SetFloat("MoveY", -1);
            animator.SetFloat("MoveX", 0);
        }
    }

    void FixedUpdate()
    {
        if (broken)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;

        if (direction == 1)
        {
            position.x = position.x + Time.deltaTime * speed;
        }
        else if(direction == 2)
        {
            position.x = position.x - Time.deltaTime * speed;
        }
        else if (direction == 3)
        {
            position.y = position.y + Time.deltaTime * speed;
        }
        else
        {
            position.y = position.y - Time.deltaTime * speed;
        }

        rigidbody2d.MovePosition(position);
    }

    
    void OnCollisionStay2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
        direction = Random.Range(1, 5);
        
    }

    public void Fix()
    {
        broken = true;
        amount += 1;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(robotFixed);
        EndGame();
    }
    void playSound()
    {
        audioSource.Play();
    }

    void EndGame()
    {
        if (amount == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    
}
