using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    private Animator anim;
    private bool dead = false;

    [Header("iFrames")]
    
    [SerializeField] private float iFramesDuration;
    [SerializeField] private float numberOffFlashes;
    private SpriteRenderer spriteRend;

    private Rigidbody2D RB;

    [Header ("Sound")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(hurtSound);
            StartCoroutine(Invulnerability());

            
        }

        else
        {
            if (!dead)
            {
                anim.SetTrigger("Die");
                RB.velocity = Vector3.zero;
                dead = true;
                SoundManager.instance.PlaySound(deathSound);


                if (GetComponent<MeleeEnemy>() != null)
                {
                    GetComponent<MeleeEnemy>().enabled = false;
                }

                dead = true;
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(7, 8,true);
        //Waiting
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRend.color = Color.red;
            yield return new WaitForSeconds(iFramesDuration/(numberOffFlashes * 2));
            
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOffFlashes * 2));
        }
        
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    private void Death()
    {
        if (dead == true && gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
            if (GetComponent<PlayerMovement>() != null)
            {
                GetComponent<PlayerMovement>().enabled = false;
            }

            if (GetComponentInParent<EnemyPatrol>() != null)
            {
                GetComponentInParent<EnemyPatrol>().enabled = false;
            }
        }
    }

    public void Respawn()
    {
        AddHealth(startingHealth);
        anim.ResetTrigger("Die");
        anim.Play("idle");
        gameObject.SetActive(true);

        StartCoroutine(Invulnerability());

        GetComponent<PlayerMovement>().enabled = true;
       
    }
}
