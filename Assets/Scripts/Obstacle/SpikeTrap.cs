using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {

        if(other.GetComponent<Player>()){
            Player player = other.GetComponent<Player>();
            player.HandleDeath();
        }
    }
}
