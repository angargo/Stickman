using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

    static public  GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    Keyboard k;
    
    void Start()
    {
        k = GameObject.FindObjectOfType<Keyboard>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition    = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {


        itemBeingDragged   = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent) transform.position = startPosition;
        /*if (transform.parent.name == "slot") print("q");//k.setSkill('q', );
        else if (transform.parent.name == "slot1") print("w"); 
        else if (transform.parent.name == "slot2") print("e"); 
        else if (transform.parent.name == "slot3") print("r"); 
        else if (transform.parent.name == "slot4") print("t"); */

    }

}
