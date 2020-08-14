using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapInfo : MonoBehaviour
{
    Ray2D _ray;
    RaycastHit2D hit;
    public LayerMask mask;
    private void FixedUpdate()
    {
        Vector2 camPos = PaletteMap.instance.GetMapPoint + new Vector3(0.5f,0.5f,0);
        Vector2 dir = Vector2.zero;//PaletteMap.instance.GetMapPoint - camPos;
        _ray = new Ray2D(camPos, dir);// PaletteMap.instance.GetMapPoint;

        hit = Physics2D.Raycast(_ray.origin, _ray.direction, 0, mask);
        if (hit.collider != null && PaletteMap.instance.DrawLimit()){
            string h = hit.collider.name;
            MapData mapData = hit.collider.transform.parent.GetComponent<MapData>();
            string speed = (mapData.speed * 100).ToString();
            string def = (mapData.defance * 100).ToString();
            MouseInfo.instance.SetData(mapData.typeName,speed,def,h);

        }

        //Debug.DrawRay(_ray.origin, Vector2.right, Color.red);
        //Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
    }
}
