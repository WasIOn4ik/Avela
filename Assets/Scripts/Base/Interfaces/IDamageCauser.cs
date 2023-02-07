using UnityEngine;

public interface IDamageCauser
{
    public string GetTitle();

    public GameObject GetCauser();

    public Damage GetDamage();
}
