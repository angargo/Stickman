using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyEquipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    public int type;

    static public GameObject draggedItem;

    public Color defaultColor = Color.white, highlightedColor, pressedColor;
    private Image myImage;

    private GameObject movedSkills; //We store the dragged objects into the 'Moved Skills' object. [maybe better 'Dragged Skills'?]

    private bool isMouseOverMe; //If cursor is over the button or not


    //Possible states of the button:
    private int myState;
    const int inInventory = 0;
    const int dragged = 1;
    const int equipped = 2;
    const int destroyed = 3;

    public ItemPanel myParent = null;
    private UISkill mySkill;
    private Player player;
    private bool wasBeingUsed;


    private float t = 0;
    private float upButtonLimit = 0.1f;
    private float clickTimeLimit = 0.5f;
    private bool clicked = false;


    public void copyMyself()
    { //We instantiate this object, set it to 'dragged', and attach it to movedSkills
        draggedItem = Instantiate(this.gameObject, this.transform.position, Quaternion.identity) as GameObject;
        draggedItem.transform.SetParent(movedSkills.transform);
        myParent = this.GetComponentInParent<ItemPanel>();
        draggedItem.GetComponent<MyEquipButton>().setParameters(dragged, myParent);
        draggedItem.name = this.gameObject.name; //To avoid the extra '(copy)' in its name.

    }


    public void startDrag()
    {
        //Creating the 'dragged version' of the object
        copyMyself();
        Destroy(this.gameObject); //Always destroy
    }

    //Button Stuff

    public void OnPointerDown(PointerEventData eventData)
    { //When I press the skill [with any button]

        if (!Input.GetMouseButtonDown(0)) return; //We only want the left button

        startCountDown();

        //Button aesthetics
        if (myState != dragged) myImage.color = pressedColor;
        else myImage.color = defaultColor;

        Invoke("startDrag", upButtonLimit);
    }

    public void OnPointerUp(PointerEventData eventData)
    { //When we stop pressing any button
        if (isMouseOverMe) myImage.color = highlightedColor; //Button aesthetics
        CancelInvoke("startDrag");
    }

    public void OnPointerEnter(PointerEventData eventData) //Cursor gets in
    {
        //Button aesthetics
        if (myState != dragged) myImage.color = highlightedColor;
        else myImage.color = defaultColor;

        isMouseOverMe = true; //Of course...
    }

    public void OnPointerExit(PointerEventData eventData)  //Cursor gets out
    {
        myImage.color = defaultColor; //Button aesthetics

        isMouseOverMe = false;
    }

    void startCountDown()
    {
        if (clicked)
        {
            if (myState == inInventory) equip();
            else putInInventory();
        }

        clicked = true;
        t = 0;
    }

    void updateCountDown()
    {
        if (clicked && t < clickTimeLimit) t += Time.deltaTime;
        else
        {
            t = 0;
            clicked = false;
        }
    }

    public void setParameters(int state, ItemPanel panel)
    { //set myState to state (+ consequences)
        myState = state;
        myParent = panel;
        if (myState == dragged)
        {
            //aesthetics
            myImage = this.GetComponent<Image>();
            myImage.color = defaultColor;

            //We don't want this object to bother our raycasts if we are dragging it
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public void putInBar(ItemPanel panel)
    { //We instantiate the object and put it as a child of the panel
        //eliminateOtherSkills(); //We eliminate all other skills in the bar with the same skillNumber
        if (panel == null) return;
        GameObject newChildren = Instantiate(this.gameObject, panel.transform.position, Quaternion.identity) as GameObject;
        newChildren.transform.SetParent(panel.transform);
        if (panel.type == 0) newChildren.GetComponent<MyEquipButton>().setParameters(inInventory, panel);
        else newChildren.GetComponent<MyEquipButton>().setParameters(equipped, panel);
        newChildren.name = this.gameObject.name;
    }

    void equip()
    {
        ItemPanel[] itemPanels = GameObject.FindObjectsOfType<ItemPanel>();
        ItemPanel firstFree = null;
        ItemPanel firstOccupied = null;
        foreach (ItemPanel itemPanel in itemPanels)
        {
            if (itemPanel.type == this.type)
            {
                if (itemPanel.isFree() && (firstFree == null || itemPanel.order < firstFree.order))
                {
                    firstFree = itemPanel;
                }
                else if (!itemPanel.isFree() && (firstOccupied == null || itemPanel.order < firstOccupied.order)) firstOccupied = itemPanel;
            }
        }
        ItemPanel targetPanel = firstFree;
        if (targetPanel == null) targetPanel = firstOccupied;
        if (targetPanel == null) return;
        targetPanel.setMySkill(this.gameObject);
    }

    void putInInventory()
    {
        ItemPanel[] itemPanels = GameObject.FindObjectsOfType<ItemPanel>();
        ItemPanel firstFree = null;
        foreach (ItemPanel itemPanel in itemPanels)
        {
            if (itemPanel.type == ItemPanel.inventory)
            {
                if (itemPanel.isFree() && (firstFree == null || itemPanel.order < firstFree.order))
                {
                    firstFree = itemPanel;
                }
            }
        }
        if (firstFree == null) return;
        firstFree.setMySkill(this.gameObject);
    }

    public void placedSuccessfully()
    {
        myState = destroyed;
    }


    void Awake()
    {
        myState = inInventory; //just in case
        myParent = this.GetComponentInParent<ItemPanel>();
        t = 0;
        clicked = false;
    }

    void Start()
    {
        //Initializing stuff
        if (myImage == null) myImage = this.GetComponent<Image>();
        myImage.color = defaultColor;
        movedSkills = GameObject.Find("Moved Skills");
        mySkill = GetComponent<UISkill>();
        player = GameObject.FindObjectOfType<Player>();
        isMouseOverMe = false;
        wasBeingUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (myState == destroyed) Destroy(this.gameObject);
        updateCountDown();
        if (myState == dragged) this.transform.position = Input.mousePosition; //obv
        if (Input.GetMouseButtonUp(0))
        {
            //The ItemPanel script is executed first, and thus it will be cloned before destroying itself whenever we drag it to a panel
            if (myState == dragged)
            {
                putInBar(myParent);
                Destroy(this.gameObject);
            }
        }

        /*if (myState == equipped)
        {

            Skill[] skills = player.GetComponentsInChildren<Skill>();

            bool usingMyself = false;
            foreach (Skill skill in skills)
            {
                if (skill.getSkillNumber() == mySkill.skillNumber)
                {
                    myImage.color = pressedColor;
                    usingMyself = true;
                    wasBeingUsed = true;
                }
            }
            if (!usingMyself && wasBeingUsed)
            {
                wasBeingUsed = false;
                if (isMouseOverMe) myImage.color = highlightedColor;
                else myImage.color = defaultColor;
            }
        }*/
    }
}
