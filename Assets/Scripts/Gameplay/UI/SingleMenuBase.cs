using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Spes.UI
{
    public class MenuBase : MonoBehaviour, IMenuBase
    {
        [Inject] protected GameplayBase gameplayBase;

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Show(bool bHideOthers)
        {

        }

        public void ShowSubmenu(bool bHideOthers)
        {
            throw new System.NotImplementedException();
        }
    }
}
