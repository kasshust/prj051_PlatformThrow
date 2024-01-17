using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemyGeneralAnimatorController : PlatformCharacterAnimatorController<PlatformEnemyBase> 
{
    public void Release() {
        m_PlatformCharacter.ReleaseObject();
    }

    public void DeadEffect() {
        // FactoryManager.Instance.GetObject<ActionGameEffect>(FactoryManager.EFFECT.EF000, transform.position, Quaternion.identity);
    }
}
