using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGLline : MonoBehaviour
{
    public Material mat;
    public Material mat2;
    Camera m_camera;

    //public GameObject MousePos;
    //Text MP_text;
    private void Start()
    {
        m_camera = GetComponent<Camera>();
        //MP_text = MousePos.GetComponentInChildren<Text>();
    }
    private void OnPostRender()
    {
        if (!mat) return;
       
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        mat.SetPass(0);
        GL.Begin(GL.LINES);
        //GL.Color(color);
        Vector3 min = Camera.main.WorldToScreenPoint(PaletteMap.instance.CellToWorldPos(new Vector3Int(0, 0, 0)));
        Vector3 max = Camera.main.WorldToScreenPoint(PaletteMap.instance.CellToWorldPos(new Vector3Int(100, 100, 0)));
        
        float Xone = (max.x-min.x) / 100;
        float Yone = (max.y -min.y) / 100;
        for (int i = 0; i <= 100; i++)
        {
            GL.Vertex3(min.x, min.y + Yone * i, 0);
            GL.Vertex3(max.x, min.y + Yone*i, 0);

            GL.Vertex3(min.x+Xone*i, min.y , 0);
            GL.Vertex3(min.x+Xone*i, max.y, 0);
        }

        for(int i = 0; i <= 100; i++)
        {
            GL.Vertex3(min.x - Xone, min.y + Yone * i, 0);
            GL.Vertex3(min.x, min.y + Yone * i, 0);

            GL.Vertex3(min.x + Xone * i,min.y-Yone, 0);
            GL.Vertex3(min.x + Xone * i, min.y, 0);
        }

        MousePostDraw(min, max, Xone, Yone);

        GL.End();
        GL.PopMatrix();
    }

    Vector3 CameraMove;

    public float CMoveSpeed = 1;

    public Vector2 minPos;
    public Vector2 maxPos;
    private void LateUpdate()
    {
        float MouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (MouseScroll!=0)
        {
            m_camera.orthographicSize -= MouseScroll*10;
            if (m_camera.orthographicSize < 15) m_camera.orthographicSize = 15;
            if (m_camera.orthographicSize > 55) m_camera.orthographicSize = 55;
        }
        if (Input.GetMouseButton(1))
        {
            CameraMove = Vector3.zero;
            CameraMove.x += Input.GetAxis("Mouse X") * (CMoveSpeed + 1.5f * (m_camera.orthographicSize / 55));
            CameraMove.y += Input.GetAxis("Mouse Y") * (CMoveSpeed + 1.5f * (m_camera.orthographicSize / 55));
           
            transform.position -= CameraMove;
            CameraMoveLimit();
        }

        //if (PaletteMap.instance.DrawLimit())
        //{
        //    MousePos.SetActive(true);
        //    MP_text.text = "x:" + PaletteMap.instance.GetMapPoint.x + ",y:" + PaletteMap.instance.GetMapPoint.y;
        //    MousePos.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition / CanvasData.instance.UIScale;
        //}
        //else { MousePos.SetActive(false); }
    }
    void CameraMoveLimit()
    {
        Vector3 camera = transform.position;
        if (camera.x <= minPos.x) camera.x = minPos.x;
        if (camera.y <= minPos.y) camera.y = minPos.y;
        if (camera.x >= maxPos.x) camera.x = maxPos.x;
        if (camera.y >= maxPos.y) camera.y = maxPos.y;
        transform.position = camera;
    }
    void MousePostDraw(Vector3 min,Vector3 max,float Xone,float Yone)
    {
        if (PaletteMap.instance.DrawLimit())
        {
            mat2.SetPass(0);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x, min.y + Yone * PaletteMap.instance.GetMapPoint.y, 1);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x + Xone, min.y + Yone * PaletteMap.instance.GetMapPoint.y, 1);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x, min.y + Yone * PaletteMap.instance.GetMapPoint.y, 1);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x, min.y + Yone * PaletteMap.instance.GetMapPoint.y + Yone, 1);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x, min.y + Yone * PaletteMap.instance.GetMapPoint.y + Yone, 1);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x + Xone, min.y + Yone * PaletteMap.instance.GetMapPoint.y + Yone, 1);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x + Xone, min.y + Yone * PaletteMap.instance.GetMapPoint.y, 1);
            GL.Vertex3(min.x + Xone * PaletteMap.instance.GetMapPoint.x + Xone, min.y + Yone * PaletteMap.instance.GetMapPoint.y + Yone, 1);
        }
    }
}
