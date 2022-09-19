using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    //获取网格创建脚本
    public GridMeshCreate gridMeshCreate;
    //控制网格元素grid是障碍的概率
    [Range(0,1)]
    public float probability;

    public Grid firstNode;
    public Grid SecondNode;

    private bool choseNode = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Run();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            PathFinding();
        }
    }
    public void PathFinding()
    {
        Vector2 startPos = new Vector2(firstNode.gridWidght,firstNode.girdHeight);
        Vector2 EndPos = new Vector2(SecondNode.gridWidght,SecondNode.girdHeight);
        AStarMgr.GetInstance().FindPath(startPos,EndPos);
    }
    private void Run()
    {
        
        gridMeshCreate.gridEvent = GridEvent;
        gridMeshCreate.CreateMesh();
    }

    /// <summary>
    /// 创建grid时执行的方法，通过委托传入
    /// </summary>
    /// <param name="grid"></param>
    private void GridEvent(GameObject obj,int w,int h)
    {
        Grid grid = obj.GetComponent<Grid>();
        //概率随机决定该元素是否为障碍
        float f = Random.Range(0, 1.0f);
        grid.color = f <= probability ? Color.red : Color.white;
        grid.isHinder = f <= probability;

        //保存格子在网格中的坐标,初始化block属性
        grid.Init(w,h);


        //模板元素点击事件
        grid.OnClick = () => {
            if (!grid.isHinder)
            {
                if(!choseNode)
                {
                    grid.color = Color.blue;
                    firstNode = grid;
                    choseNode = true;
                }
                else
                {
                    grid.color = Color.cyan;
                    SecondNode = grid;
                }
            }
        };

    }
}
