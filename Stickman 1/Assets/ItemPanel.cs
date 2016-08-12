using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int order;
    public bool trash;

    private bool inside;

    public int type;
    public const int inventory = 0;
    public const int weapon = 1;
    public const int armor = 2;

    //Same stuff as in MyEquipButton

    public void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inside = true;
    }

    // Use this for initialization
    void Start()
    {
        //Initializing stuff
        inside = false;
    }

    public void setMySkill(GameObject o)
    {

        //Just in case
        MyEquipButton skill = o.GetComponent<MyEquipButton>();
        if (skill == null) return;

        //Same kind of object
        if (type > 0 && skill.type != type) return;

        //If the dragged item comes from another position of the bar, swap!
        MyEquipButton mySkill = this.GetComponentInChildren<MyEquipButton>();
        if (mySkill != null)
        {
            ItemPanel panel = o.GetComponent<MyEquipButton>().myParent;
            if (panel != null) mySkill.putInBar(panel);
        }

        destroyAllMyChildren(); //Whenever we drag a new skill, we destroy the previous one

        //We make a copy of the skill and attach it here
        skill.putInBar(this);
        skill.placedSuccessfully();
    }

    void destroyAllMyChildren()
    {
        MyEquipButton[] skills = this.GetComponentsInChildren<MyEquipButton>();
        foreach (MyEquipButton skill in skills) Destroy(skill.gameObject);
    }

    public bool isFree()
    {
        return this.transform.childCount <= 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (inside)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (MyEquipButton.draggedItem != null)
                { //Dragging an actual skill
                    setMySkill(MyEquipButton.draggedItem);
                }
            }
        }
        if (trash) destroyAllMyChildren();
    }
}
