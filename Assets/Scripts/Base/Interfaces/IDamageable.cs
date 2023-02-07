using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public DamageableInfo GetInfo();

    public void ApplyDamage(Damage dmg);
}
