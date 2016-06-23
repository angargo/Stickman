﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class slot : MonoBehaviour, IDropHandler {

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0) return transform.GetChild(0).gameObject;
            return null;
        } 
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!item)
        {
            DragHandeler.itemBeingDragged.transform.SetParent(transform);
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}