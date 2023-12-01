using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrownController : MonoBehaviour
{

    
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    
    
    

    void Start()
    {
        scoreDisplay.text = "You Finished In: " + (int)TimeTracker.instance?.currentTime + " Seconds!";
    }

   



}
