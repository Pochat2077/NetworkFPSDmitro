using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon<TData> : Item<TData> where TData : WeaponData
{
    // Start is called before the first frame update
    public abstract override void Use();
    protected abstract override void Awake();
    protected abstract override void Start();
    protected abstract override void Update();
}
