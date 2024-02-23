using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;
    
    [Header("Timers")]

    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;
    private Health player;
    private bool takenDamage;

    [SerializeField] private AudioClip FireSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFireTrap());

            player = collision.GetComponent<Health>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && player != null && !takenDamage)
        {
            player.TakeDamage(damage);
            takenDamage = true;
            
        }
                
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
        takenDamage = false;
    }
    private IEnumerator ActivateFireTrap()
    {
        triggered = true;

        SoundManager.instance.PlaySound(FireSound);

        spriteRend.color = Color.red;
        yield return new WaitForSeconds(activationDelay);
        
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("Activated", true);
        yield return new WaitForSeconds(activeTime);
        
        
        active = false;
        triggered = false;
        anim.SetBool("Activated", false);
    }
}
