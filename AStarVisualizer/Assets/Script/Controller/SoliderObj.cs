using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum E_SoliderType
{
    Hero,
    //melee
    warrior,
    //range
    Hunter,
    witChDoctor,
    WyvernRider,
}
public class SoliderObj : MonoBehaviour
{
    public E_SoliderType soliderType;
    private CapsuleCollider myCollider;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private GameObject footEffect;
    bool isChose = false;
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
        myCollider = this.GetComponent<CapsuleCollider>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        footEffect = this.transform.Find("FootEffect").gameObject;
        footEffect.SetActive(false);
    }
    //animation logic

    //if is chose -> show effect

    //Navmesh agent

    void Update()
    {
        animator.SetBool("IsMove",  navMeshAgent.velocity.magnitude > 0?true:false);
    }
    public void SetChoseState(bool chose)
    {
        isChose = chose;
        if(isChose)
        {
        
            footEffect.SetActive(true);
        }
        else
        {
            footEffect.SetActive(false);   
        }
    }
    public void Move(Vector3 pos)
    {
        navMeshAgent.SetDestination(pos);
    }
}
