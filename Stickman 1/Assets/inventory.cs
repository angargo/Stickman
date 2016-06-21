using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class inventory : MonoBehaviour {
    [SerializeField]    Transform slots;
    [SerializeField]    Text inventoryText;
	// Use this for initialization
	void Start () {
        HasChanged();
	}
	
	public void HasChanged()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(" - ");
        foreach(Transform slotTransform in slots)
        {
            GameObject item = slotTransform.GetComponent<slot>().item;
            if(item)
            {
                builder.Append(item.name);
                builder.Append(" - ");
            }
        }
        inventoryText.text = builder.ToString();
    }
}


namespace UnityEngine.EventSystems
{
    public interface IHasChanged: IEventSystemHandler
    {
        void HasChanged();
    }
}