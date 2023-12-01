using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BuzzsawTrap : MonoBehaviour
{
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private float speed = 3.0f, spinSpeed = Mathf.PI*10f;
    [SerializeField] bool isCrown = false;

    private int currentPointIndex = 0;
    private float maxDistance = .1f;
    private float currentSpinAmount = 0;

    private void Start() {
        
    }

    private void Update() {
        Spin();
        OscillateBewteenPoints();
    }

    private void OscillateBewteenPoints(){
        

        if(points.Count <= 0){ 
            Debug.LogError("No Waypoints Found!");
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, points[currentPointIndex].position,(speed * Time.deltaTime));
        

        if(Vector2.Distance(transform.position, points[currentPointIndex].position) < maxDistance){

            currentPointIndex = (currentPointIndex + 1) % points.Count;
        }
        
    }

    private void Spin(){
        currentSpinAmount += Time.deltaTime*spinSpeed;

        if(currentSpinAmount >= 360){
    
            currentSpinAmount = 0;
        }

        transform.rotation = Quaternion.Euler(0,0,currentSpinAmount);

    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.GetComponent<Player>() && !isCrown){
            Player player = other.GetComponent<Player>();
            player.HandleDeath();
        }
    }
    
    private void OnDrawGizmos() {

        if(points.Count <= 0){ return;}
        
        Gizmos.color = Color.red;

        if(points.Count > 2) {Gizmos.DrawLine(points[points.Count-1].position, points[0].position);}

        for (int i = 0; i < points.Count; i++)
        {
            if(i < points.Count-1){
                Gizmos.DrawLine(points[i].position, points[i+1].position);         
            }
        }
    }

}
