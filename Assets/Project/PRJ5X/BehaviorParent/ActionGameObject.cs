using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionGameObject : ActionGameBehavior<ActionGameObject> { 

    public FactoryManager.OBJECT m_ObjectEnum;

    override public void ReleaseObject()
    {
        m_FactoryManager.ReleaseObject(m_ObjectEnum, this.gameObject);
    }


}
