using System;
using Demo;
using UnityEngine;
using Random = System.Random;

public class MonsterAttackingEvent : MonoBehaviour
{
    private Animator mAnimator;

    private void Awake()
    {
//        mAnimator = this.GetComponent<Animator>();
//        RuntimeAnimatorController mRuntimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
//        AnimationEvent newEvent = new AnimationEvent();
//        newEvent.functionName = "Attacking";
//        AnimationClip[] clips = mRuntimeAnimatorController.animationClips;
//        for (int i = 0; i < clips.Length; i++)
//        {
//            if (clips[i].name == "BoxerAttack1")
//            {
//                mRuntimeAnimatorController.animationClips[i].AddEvent(newEvent);
//            }
//        }
//        mAnimator.Rebind();
    }

    public void Attacking()
    {
        Debug.Log("monster attacking!");
        if (GetComponent<RoleAnimator>().lastEnemy != null)
        {
            GetComponent<RoleAnimator>().lastEnemy.GetComponent<RoleAnimator>().BeAttack(10);
        }
    }
}