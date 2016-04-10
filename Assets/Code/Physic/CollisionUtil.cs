using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CollisionUtil 
{
    public static Vector3 CalculateJiGuanHitPoint(Character target, GameObject collider)
    {
        Vector3 kColliderDir = Vector3.zero;

        CapsuleCollider kTargetCollider = target.GetComponent<CapsuleCollider>();
        if (!kTargetCollider)
        {
            return Vector3.zero;
        }

        if (collider != null)
        {
            // 物理碰撞
            kColliderDir = collider.transform.position - target.transform.position;
            kColliderDir.Normalize();
        }
        else
        {
            // 范围伤害之类
            kColliderDir = CharacterPlayer.sPlayerMe.transform.position - target.transform.position;
            kColliderDir.Normalize();
        }

        return target.transform.position + kColliderDir * kTargetCollider.radius;
    }
}

