using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    private float timer;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = 0;
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1.5f)
        {
            Destroy(gameObject);
        }
    }

    

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if(e != null)
        {
            e.Fix();
        }
        Destroy(gameObject);
    }
    
}
