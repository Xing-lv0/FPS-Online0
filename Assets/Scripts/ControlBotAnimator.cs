using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBotAnimator : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed;
    public bool alerted;
    public bool death;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimator();
    }

    //�ƶ������䡢������
    void SetAnimator()
    {
        if (animator == null)
            return;

        animator.SetFloat("MoveSpeed", moveSpeed);
        animator.SetBool("Alerted", alerted);
        animator.SetBool("Death", death);
    }

    //������
    public void TriggerAttack()
    {
        if (animator == null)
            return;

        animator.SetTrigger("Attack");
    }

    //���ˣ�
    public void TriggerOnDamage()
    {
        if (animator == null)
            return;

        animator.SetTrigger("OnDamaged");
    }
}
