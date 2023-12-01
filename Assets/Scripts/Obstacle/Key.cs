using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public event Action keyCollectedEvent;
    private Animator anim;
    private SpriteRenderer sr;
    private CircleCollider2D cd;

    [SerializeField] private GameObject collectedSprite;

    private void Awake() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Start() {
        collectedSprite.SetActive(false);
        
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.GetComponent<Player>())
        {
            PickupKey();
        }
    }

    private void PickupKey()
    {   
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[5].audioClip); // UI Select

        cd.enabled = false;
        keyCollectedEvent?.Invoke();
        collectedSprite.SetActive(true);
        anim.enabled = false;
        sr.enabled = false;
        
    }
}
