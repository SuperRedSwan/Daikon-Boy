using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private float tuningValue = 2f;
    [SerializeField] private bool lockY;    
    private Player player;

    private void Awake() {
        player = GetComponentInParent<Player>();
    }

    private void Update() {
        
        if(lockY) transform.position = Vector3.Lerp(transform.position,new Vector3(player.transform.position.x, 0,player.transform.position.z), Time.deltaTime * tuningValue);
        
    }
}
