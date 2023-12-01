using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ElementSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] float verticalMoveAmount = 30f;
    [SerializeField] float moveTime = .1f;
    [Range(0,2), SerializeField] float scaleMultiplier = 1.1f;

    private Vector3 originalScale;
    private Vector3 originalPos;

    private Coroutine moveCoroutine;

    private void Start() {
        originalScale = transform.localScale;
        originalPos = transform.position;
    }

    private IEnumerator MoveElement(bool isStartingAnimation){


        Vector3 finalScale, finalPos;
        
        float elapsedTime = 0f;

        while(elapsedTime < moveTime){

            elapsedTime += Time.deltaTime;

            if(isStartingAnimation){
                finalPos = originalPos + new Vector3(0f, verticalMoveAmount, 0f);
                finalScale = originalScale * scaleMultiplier;
            }
            else{
                finalPos = originalPos;
                finalScale = originalScale;
            }

            // Calculate lerped amounts
            Vector3 lerpedPos = Vector3.Lerp(transform.position, finalPos, (elapsedTime / moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, finalScale, (elapsedTime / moveTime));

            // Apply lerped values to element
            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
        
        

        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // select card
        eventData.selectedObject = gameObject;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(eventData.selectedObject == gameObject) eventData.selectedObject = null;
    }


    public void OnSelect(BaseEventData eventData)
    {

        if(moveCoroutine != null){ StopCoroutine(moveCoroutine);}

        moveCoroutine = StartCoroutine(MoveElement(true));
        
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips[1].audioClip); // UI Select

        ElementSelectionManager.instance.lastSelectedObject = gameObject;

        for (int i = 0; i < ElementSelectionManager.instance.UIElements.Length; i++)
        {
            if(ElementSelectionManager.instance.UIElements[i] == gameObject){

                ElementSelectionManager.instance.lastSelectedIndex = i; 
                return;
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(moveCoroutine != null){ StopCoroutine(moveCoroutine);}

        moveCoroutine = StartCoroutine(MoveElement(false));
        

    }

}

