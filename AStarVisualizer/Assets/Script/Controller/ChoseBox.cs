using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseBox : MonoBehaviour
{
    List<SoliderObj> chosenTeam = new List<SoliderObj>();
    public int MaxTeamNum = 12;
    //Draw line
    private Vector3 StartPoint;
    private Vector3 EndPoint;
    private Vector3 rightupPoint;
    private Vector3 leftbottomPoint;
    public int lineDepth = 5;
    private LineRenderer line;
    bool isChosing = false;
    //raycast
    public LayerMask layerMask;
    private RaycastHit hitinfo;
    private Vector3 beginPoint;
    private Vector3 center;
    private Vector3 halfExtend;
    //move control
    private List<Vector3> targetsPos = new List<Vector3>();
    //
    private Vector3 frontPos = Vector3.zero;
    Vector3 nowForward;
    Vector3 nowRigth;
    public float soldierOffset = 2;

    void Start()
    {
        line = this.GetComponent<LineRenderer>();
    }

    //Draw Chosen line
    private void DrawLine()
    {
        line.positionCount = 4;
        //draw line
        EndPoint = Input.mousePosition;

        EndPoint.z = lineDepth;
        StartPoint.z = lineDepth;
        rightupPoint.x = EndPoint.x;
        rightupPoint.y = StartPoint.y;
        rightupPoint.z = lineDepth;
        leftbottomPoint.x = StartPoint.x;
        leftbottomPoint.y = EndPoint.y;
        leftbottomPoint.z = lineDepth;

        line.SetPosition(0,Camera.main.ScreenToWorldPoint(StartPoint));
        line.SetPosition(1,Camera.main.ScreenToWorldPoint(rightupPoint));
        line.SetPosition(2,Camera.main.ScreenToWorldPoint(EndPoint));
        line.SetPosition(3,Camera.main.ScreenToWorldPoint(leftbottomPoint));
    }
    private void ClearTeam()
    {
        foreach(SoliderObj soilder in chosenTeam)
        {
            if(soilder!=null)
            {
                soilder.SetChoseState(false);
            }
        }
        chosenTeam.Clear();
    }
    //add chosen team in list
    private void ChoseTeam()
    {
        ClearTeam();
        Collider[] colliders = Physics.OverlapBox(center,halfExtend);
        foreach(Collider soilder in colliders)
        {
            SoliderObj obj = soilder.transform.GetComponent<SoliderObj>();
            if(chosenTeam.Count < MaxTeamNum)
            if(obj!=null)
            {
                obj.SetChoseState(true);
                chosenTeam.Add(obj);
            }
        }
        //sort
        SortTeam();
    }
    //Select logic
    private void SelectSodiler()
    {
         if(Input.GetMouseButtonDown(0))
        {
            line.positionCount = 0;
            StartPoint = Input.mousePosition;
            isChosing = true;

            //box raycast start point
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hitinfo,1000,layerMask))
            {
                beginPoint = hitinfo.point;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            //box raycast 
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hitinfo,1000,layerMask))
            {
                center = new Vector3((hitinfo.point.x + beginPoint.x)/2,1,(hitinfo.point.z + beginPoint.z)/2);
                //half of a box collider
                halfExtend = new Vector3(Mathf.Abs(hitinfo.point.x - beginPoint.x)/2,2,Mathf.Abs(hitinfo.point.z-beginPoint.z)/2);
            }
            ChoseTeam();
            isChosing = false;
            //clear last pos
            frontPos = Vector3.zero;
        }

        if(isChosing)
        {
            DrawLine(); 
        }
        else
        {
            //clearline
            line.positionCount = 0;
        }
    }
    private void SingleSoilderChose()
    {
        ClearTeam();
    }
    //movecontrol
    private void GetTargetPos(Vector3 targetPos)
    {
        nowForward = Vector3.zero;
        nowRigth = Vector3.zero;
        //if frontpos exist use instantly,if not pick the firsr solider
        if(frontPos != Vector3.zero)
        {
            nowForward = (targetPos - frontPos).normalized;
        }
        else
        {
            nowForward = (targetPos - chosenTeam[0].transform.position).normalized;
        }
        nowRigth = Quaternion.Euler(0,90,0) * nowForward;

        SwitchTeamFormation(targetPos);

    }
    private void SwitchTeamFormation(Vector3 targetPos)
    {
        switch (chosenTeam.Count)
        {
            case 1:
                targetsPos.Add(targetPos);
                break;
            case 2:
                targetsPos.Add(targetPos + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos - nowRigth * soldierOffset / 2);
                break;
            case 3:
                targetsPos.Add(targetPos);
                targetsPos.Add(targetPos + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowRigth * soldierOffset);
                break;
            case 4:
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2 - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2 + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos - nowForward * soldierOffset / 2 - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos - nowForward * soldierOffset / 2 + nowRigth * soldierOffset / 2);
                break;
            case 5:
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2 - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2 + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset / 2 - nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset / 2 + nowRigth * soldierOffset);
                break;
            case 6:
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2 - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset / 2 + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset / 2 - nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset / 2 + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset / 2);
                break;
            case 7:
                targetsPos.Add(targetPos + nowForward * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowRigth * soldierOffset);
                targetsPos.Add(targetPos);
                targetsPos.Add(targetPos - nowForward * soldierOffset);
                break;
            case 8:
                targetsPos.Add(targetPos + nowForward * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowRigth * soldierOffset);
                targetsPos.Add(targetPos);
                targetsPos.Add(targetPos - nowForward * soldierOffset - nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset + nowRigth * soldierOffset);
                break;
            case 9:
                targetsPos.Add(targetPos + nowForward * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowRigth * soldierOffset);
                targetsPos.Add(targetPos + nowRigth * soldierOffset);
                targetsPos.Add(targetPos);
                targetsPos.Add(targetPos - nowForward * soldierOffset - nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset + nowRigth * soldierOffset);
                targetsPos.Add(targetPos - nowForward * soldierOffset);
                break;
            case 10:
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos - nowForward * soldierOffset - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowForward * soldierOffset + nowRigth * soldierOffset * 1.5f);
                break;
            case 11:
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos - nowForward * soldierOffset - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowForward * soldierOffset + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowForward * soldierOffset);
                break;
            case 12:
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowForward * soldierOffset - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos + nowForward * soldierOffset + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos + nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos - nowForward * soldierOffset - nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowForward * soldierOffset + nowRigth * soldierOffset * 1.5f);
                targetsPos.Add(targetPos - nowForward * soldierOffset - nowRigth * soldierOffset / 2);
                targetsPos.Add(targetPos - nowForward * soldierOffset + nowRigth * soldierOffset / 2);
                break;
        }

    }
    private void MoveControl()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(chosenTeam.Count == 0)
            return;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hitinfo,1000,layerMask))
            {
                //calculate angle
                //
                Vector3 newVector = (hitinfo.point - chosenTeam[0].transform.position).normalized;
                //队列中第一个士兵作为老朝向
                Vector3 oldVector = chosenTeam[0].transform.forward;                
                if(Vector3.Angle(newVector,oldVector)>60)
                {
                    SortTeam();
                }
                //move
                GetTargetPos(hitinfo.point);
                for(int i=0; i < chosenTeam.Count; i++)
                {
                    
                    chosenTeam[i].Move(targetsPos[i]);      
                }
            }
            targetsPos.Clear();
        }
    }
    private void SortTeam()
    {
        //sort based on type first
        //if same type sort based on distance
         chosenTeam.Sort((a, b)=>{
            if(a.soliderType < b.soliderType)
            return -1;
            else if(a.soliderType == b.soliderType)
            {
                if(Vector3.Distance(a.transform.position,hitinfo.point) <=
                Vector3.Distance(b.transform.position,hitinfo.point))
                {
                    return -1;
                }
                else
                {
                    return 1;
                }

            }
            else
            return 1;
        });
    }

    void Update()
    {
       SelectSodiler();
        MoveControl();
    }
}
