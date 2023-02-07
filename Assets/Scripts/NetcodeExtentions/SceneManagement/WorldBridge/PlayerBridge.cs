using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface SemiNetworkedHandler
{
    public void Interact(ulong clientID, string title, string args);

    public bool CanInteract(ulong clientID);
}

public class PlayerBridge : MonoBehaviour
{
    protected Dictionary<Type, SemiNetworkedHandler> handlers;

    public void RegisterType(Type t, SemiNetworkedHandler handler)
    {
        Assert.IsNull(t, "����������� ������ ����, �� ��� NULL");
        Assert.IsNull(handler, $"����������� ������ ���� {t}, �� handler NULL");
        Assert.IsTrue(handlers.ContainsKey(t), $"������� ���������������� ���� ��� {t} ������");

        handlers.Add(t, handler);
    }
}
