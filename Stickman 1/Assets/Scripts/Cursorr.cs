using UnityEngine;
using System.Collections;

public class Cursorr : MonoBehaviour
{


    private int status = 0;
    private int def = 0;
    private int atack = 1;
    private int cast = 2;
    private int talk = 3;

    private Ray ray;
    private RaycastHit2D[] rcArray;
    private Enemy enemy;
    private bool floorFound;
    private Vector3 point = Vector3.zero;

    private Player player;
    public Texture2D[] cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public TextAsset imageAssert;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
        enemy = null;
        floorFound = false;
        point = Vector3.zero;

        changeCursor(def);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] rcArray = Physics2D.GetRayIntersectionAll(ray);
        enemy = null;
        floorFound = false;
        point = Vector3.zero;

        if (Input.GetMouseButton(1))
        {
            OnMouseRightDown();
        }

        foreach (RaycastHit2D rc in rcArray)
        {
            if (enemy == null) enemy = rc.collider.gameObject.GetComponentInParent<Enemy>();
            if (isFloor(rc) && enemy == null)
            {
                point = rc.point;
                floorFound = true;
            }
        }
        //if (Input.GetKeyDown(KeyCode.Space)) {                         changeCursor(cast);            status = cast; }
        if (enemy != null && status != atack)
        {
            changeCursor(atack); status = atack;
        }
        else if (floorFound && status != def && enemy == null)
        {
            changeCursor(def); status = def;
        }

    }

    void changeCursor(int st)
    {
        Cursor.SetCursor(cursorTexture[st], hotSpot, cursorMode);
    }

    bool isFloor(RaycastHit2D rc)
    {
        return rc.collider.gameObject.tag == "Floor";
    }

    void OnMouseRightDown()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        rcArray = Physics2D.GetRayIntersectionAll(ray);
        enemy = null;
        floorFound = false;
        point = Vector3.zero;
        foreach (RaycastHit2D rc in rcArray)
        {
            if (enemy == null) enemy = rc.collider.gameObject.GetComponentInParent<Enemy>();
            if (isFloor(rc))
            {
                point = rc.point;
                floorFound = true;
            }
        }
        if (floorFound)
        {
            player.MoveToPosition(point);
        }
        if (enemy != null)
            player.attackEnemy(enemy.GetComponent<Character>());
    }

}
