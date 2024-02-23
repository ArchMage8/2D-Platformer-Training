using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private float healthValue;
    private float originalY;
    private Vector2 floatY;
    [SerializeField] private AudioClip HealthSound;

    public float floatStrength;

    private void Start()
    {
        this.originalY = this.transform.position.y;
    }

    private void Update()
    {
        floatY = transform.position;
        floatY.y = (Mathf.Sin(Time.time) * floatStrength);
        transform.position = floatY;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(HealthSound);
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
