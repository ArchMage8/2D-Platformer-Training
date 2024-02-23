using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private Transform startCheckpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;

   
    private void Awake()
    {
        playerHealth = GetComponent<Health>();
;       currentCheckpoint = startCheckpoint;

    }

    private void Respawn()
    {
        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            Debug.Log("HHi");
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; 
            collision.GetComponent<Animator>().SetTrigger("Activate");
            Debug.Log(currentCheckpoint.transform);
        }
    }
    private void Update()
    {
        Debug.Log(currentCheckpoint.position);
        //Debug.Log(startCheckpoint.position +"Start");
    }
}
