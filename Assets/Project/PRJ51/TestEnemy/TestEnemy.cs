using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : PlatformEnemyBase
{

    public override ActionGameCharacterBase CreateInit()
    {
        base.CreateInit();
        return this;
    }

    protected override void Update()
    {
        base.Update();

        UpdateAI();

        UpdateControl();
        UpdateCollisionWithFloorCeil();
        UpdateXDirection();

        // アニメーション・効果音
        UpdateBaseAnimatorParam();
        // UpdateAnimation();
    }

    private void UpdateControl()
    {
        CalculateVelocity();
        HandleWallSliding();
        Move();
    }

    private void Move()
    {
        m_Controller.Move(m_Velocity * Time.deltaTime * m_BaseMotionSpeed, false);
    }

    public override void ChatchImpactReply(ref PlatformActionManager.ReplyInfo replyInfo)
    {
    }

    public override void Damage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, GameObject g = null)
    {
    }

    public override void Dead()
    {
        Destroy(gameObject);
    }

    public bool PlayJump()
    {
        //　壁接触時
        if (m_WallSliding)
        {
            if (Mathf.Abs(m_DirectionalInput.x) < 0.05f) //　x軸入力無し
            {
                m_Velocity.x = -m_WallDirX * m_WallJumpOff.x;
                m_Velocity.y = m_WallJumpOff.y;

                return true;
            }
            else if (Mathf.Sign(m_WallDirX) == Mathf.Sign(m_DirectionalInput.x)) // 壁方向
            {
                m_Velocity.x = -m_WallDirX * m_WallJumpClimb.x;
                m_Velocity.y = m_WallJumpClimb.y;

                return true;
            }
            else //逆側
            {
                m_Velocity.x = -m_WallDirX * m_WwallLeap.x;
                m_Velocity.y = m_WwallLeap.y;

                return true;
            }
        }

        //　接地時
        if (m_Controller.collisions.below)
        {
            if (m_Controller.collisions.slidingDownMaxSlope)　// 坂を降りてる
            {
                if (m_DirectionalInput.x != -Mathf.Sign(m_Controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    m_Velocity.y = m_MaxJumpVelocity * m_Controller.collisions.slopeNormal.y;
                    m_Velocity.x = m_MaxJumpVelocity * m_Controller.collisions.slopeNormal.x;

                    return true;

                }
            }
            else
            {
                m_Velocity.y = m_MaxJumpVelocity;

                return true;
            }
        }

        return false;
    }

    protected override void CatchActionSetting(GameObject o)
    {
        o.transform.position = transform.position;
    }

    protected override void ThrowActionSetting(Vector2 moveValue)
    {
        m_ThrowProperty.Velocity = moveValue * 10.0f;
        m_ThrowProperty.AttackSet = PlatformActionManager.AttackSet.EnemyA;
    }
}
