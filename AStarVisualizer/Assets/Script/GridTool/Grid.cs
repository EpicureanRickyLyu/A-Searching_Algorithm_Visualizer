using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum E_Node_Type
{
    Stop,
    Walk,
}
public class Grid : MonoBehaviour
{
    public int gridWidght;
    public int girdHeight;
    public bool isHinder;
    public Color color;
    public Action OnClick;    
    //寻路消耗
    public float f=0;
    //离起点的距离
    public float g=0;
    //离终点的距离
    public float h=0;
    //父对象
    public Grid father;
        //格子的类型
    public E_Node_Type type;

    public void Init(int w,int h)
    {
        gridWidght = w;
        girdHeight = h;
        type = isHinder?E_Node_Type.Stop:E_Node_Type.Walk;
    }

    //当网格地图比较大时，每帧更新模板颜色比较消耗性能，可以修改为通过事件触发
    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().material.color=color;
    }
    //委托绑定模板点击事件
    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }

}
