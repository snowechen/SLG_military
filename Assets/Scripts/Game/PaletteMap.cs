/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public enum BrushStatus
{
    empty,
    DrawMode,
    DeleteMode,
}
public class PaletteMap : MonoBehaviour
{
    public static PaletteMap instance;
    Tilemap tilemap;
    public Tile[] tiles;
    public Texture2D[] cursors;
    public Vector3Int GetMapPoint
    {
        get
        {
            return tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public BrushStatus brushType;
    public Vector3 CellToWorldPos(Vector3Int pos)
    {
        return tilemap.CellToWorld(pos);   
    }
    public int index { set; get; }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        bool MouseUI = EventSystem.current.IsPointerOverGameObject();
        if (!MouseUI)
        {
            switch (brushType)
            {
                case BrushStatus.DrawMode:
                    if (Input.GetMouseButtonDown(0) && DrawLimit())
                    {
                        tilemap.SetTile(GetMapPoint, tiles[index]);
                        brushType = BrushStatus.empty;
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                    }
                    break;
                case BrushStatus.DeleteMode:
                    if (Input.GetMouseButtonDown(0) && DrawLimit())
                    {
                        tilemap.SetTile(GetMapPoint, null);
                        //Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                    }
                    break;
            }
        }

        if (brushType == BrushStatus.DeleteMode && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1)))
        {
            brushType = BrushStatus.empty;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    

    public bool DrawLimit()
    {
        return GetMapPoint.x >= 0 && GetMapPoint.y >= 0 && GetMapPoint.x < 100 && GetMapPoint.y < 100;
    }

    public void BrushDraw(int index)
    {
        Vector2 CurPos = Vector2.zero;
        this.index = index;
        if (index < 10)
        {
            brushType = BrushStatus.DrawMode;
        }
        else if(index == 10) { brushType = BrushStatus.DeleteMode; CurPos = new Vector2(10, 15); }
        Cursor.SetCursor(cursors[index], CurPos, CursorMode.ForceSoftware);
    }
}
