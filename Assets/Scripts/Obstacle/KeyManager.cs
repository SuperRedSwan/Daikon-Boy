using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{   

    [SerializeField] GameObject door, doorFinalPos;
    [SerializeField] private float doorMoveTime = 10f;
    [SerializeField] private List<Key> keys = new List<Key>();
    private int collectedKeys = 0;

    private void Start() {
        
        
        foreach(Key key in keys){
            key.keyCollectedEvent += CollectKey;
        }
    }

    private void CollectKey(){
        collectedKeys++;

        if(collectedKeys >= keys.Count){
            OpenDoor();
        }
    }

    private void OpenDoor(){
        // disable door collider
        // play opening animation

        door.GetComponent<BoxCollider2D>().enabled = false;
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[7].audioClip);
        StartCoroutine(MoveDoor());
        
    }

    private IEnumerator MoveDoor(){


        Vector3 finalPos;
        
        float elapsedTime = 0f;

        while(elapsedTime < doorMoveTime){

            elapsedTime += Time.deltaTime;


            
            
            finalPos = new Vector3(door.transform.position.x, doorFinalPos.transform.position.y, door.transform.position.z);
            
            Vector3 lerpedPos = Vector3.Lerp(door.transform.position, finalPos, (elapsedTime / doorMoveTime));
 
            door.transform.position = lerpedPos;
            
            

            yield return null;
        }
        
        

        
    }
}
