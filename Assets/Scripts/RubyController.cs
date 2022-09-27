using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RubyController : MonoBehaviour
{
    // Start is called before the first frame update
    private int maxHealth = 10;
    public int _maxHealth { get { return maxHealth; } }
    public int currentHealth;
    public int _currentHealth { get { return currentHealth; } }
    private float speed = 3f;

    Rigidbody2D rigidbody2d;

    private float horizontal;
    private float vertical;

    private float timeDistance = 1f;
    bool isDistance;
    float timer;

    private float timeInvincible = 2f;
    bool isInvincible;
    float invincibleTimer;

    public GameObject projectilePrefab;

    public AudioClip playerHit;
    public AudioClip throwCog;
    public AudioClip collectClip;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    public bool broken = false;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        


        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if(isDistance)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                isDistance = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                JambiController controllerJB = hit.collider.GetComponent<JambiController>();
                if(controllerJB != null)
                {
                    controllerJB.DisplayDialog();
                }
            }
        }

        if (horizontal != 0 || vertical != 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }
        else
        {
            audioSource.Pause();
        }

        if (currentHealth == 0)
        {
            broken = true;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
        
        
    }

    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if (isInvincible) { return; }
            animator.SetTrigger("Hit");
            invincibleTimer = timeInvincible;
            isInvincible = true;
            audioSource.PlayOneShot(playerHit);
        }
        if(amount > 0)
        {
            audioSource.PlayOneShot(collectClip);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UiHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        if (!isDistance)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
            audioSource.PlayOneShot(throwCog);
            animator.SetTrigger("Launch");
            isDistance = true;
            timer = timeDistance;
        }
        
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    
    
    
}
