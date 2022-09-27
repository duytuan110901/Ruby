using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{   
    public GameObject pickUpEffectPrefab;
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
            if (controller._currentHealth < controller._maxHealth)
            {
                controller.ChangeHealth(1);
                Instantiate(pickUpEffectPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
