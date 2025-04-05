using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item<TData> : MonoBehaviour, IUsable where TData : ItemData
{
    [SerializeField]
    protected TData data;
    [SerializeField]
    protected GameObject itemObject;

    public GameObject ItemObject 
    {
        get {return itemObject;}
    }

    public abstract void Use();
    protected abstract void Awake();
    protected abstract void Start();
    protected abstract void Update();



}
