using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionGameUtility : ActionGameBehavior<ActionGameUtility> { 

    public FactoryManager.UTILITY m_UtilityEnum;

    override public void ReleaseObject()
    {
        m_FactoryManager.ReleaseObject(m_UtilityEnum, this.gameObject);
    }


}
