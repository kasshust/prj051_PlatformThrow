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
        m_Controller.Move(m_Velocity * Time.deltaTime * m_MotionSpeed, false);
    }

    public override void ChatchImpactReply(ref PlatformActionManager.ReplyInfo replyInfo)
    {
    }

    public override void Damage(ref PlatformActionManager.AttackInfo attackInfo, RaycastHit2D hit, GameObject g = null)
    {
    }
}
