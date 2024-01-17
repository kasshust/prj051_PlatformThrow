using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

interface IActionGameBehavior<T>
{
    abstract public void ReleaseObject();
    abstract public T CreateInit();
}

public abstract class ActionGameBehavior<T> : MonoBehaviour, IActionGameBehavior<T>
{
    protected FactoryManager m_FactoryManager;

    abstract public void ReleaseObject();
    abstract public T CreateInit();

    [ReadOnly]
    public Guid m_ID;

    private void Awake()
    {
        m_FactoryManager = FactoryManager.Instance;
        Wake();
    }
    abstract protected void Wake();

    // プールから生成時に初期化
    public T Init()
    {
        CreateID();
        AdditonalCreateInit();
        return CreateInit();
    }

    //　初期化時の追加処理
    protected virtual void  AdditonalCreateInit() {
    
    }

    private void CreateID() {
        m_ID = Guid.NewGuid();

    }

    virtual protected void Start(){
        Init();
    }
    

    private void ForceDestroy()
    {
        Destroy(this.gameObject);

    }
}
