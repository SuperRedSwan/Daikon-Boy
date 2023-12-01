using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugObject : MonoBehaviour
{
    [SerializeField, TextArea(5,10)] private string debugText;

    public void Log(string _text, Vector3 _position, string _objectName = "Default Object Name"){
        debugText = ("Name: "+ _objectName + "\n" + _text + "\n" + "At: " + _position + "\n" + "Input: " + InputReader.instance.moveDirVector);
    } 
}
