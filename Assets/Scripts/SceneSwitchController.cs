using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneSwitchController : MonoBehaviour
{
    [Inject] protected WorldCompositor worldCompositor;

    public void OnTriggerEnter(Collider other)
    {
        var chunk = other.GetComponent<ChunkNode>();
        if (chunk)
        {
            Debug.Log($"����� {gameObject.name} �������� {other.gameObject.name} �� ��������� {chunk.nodeName}");
            foreach (var sc in chunk.ensureLoaded)
            {
                worldCompositor.MarkNeeded(sc, gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var chunk = other.GetComponent<ChunkNode>();
        if (chunk)
        {
            foreach (var sc in chunk.ensureLoaded)
            {
                worldCompositor.MarkUnneeded(sc, gameObject);
            }
        }
    }
}
