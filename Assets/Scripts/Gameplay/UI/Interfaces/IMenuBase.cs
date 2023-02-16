using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuBase
{
    /// <summary>
    /// Для сингл вызывается на клиенте, для Нет вызывается на сервере по запросу
    /// </summary>
    public void Show(bool bHideOthers);

    /// <summary>
    /// Для сингл вызывается на клиенте, для Нет вызывается на сервере по запросу
    /// </summary>
    public void ShowSubmenu(bool bHideOthers);

    /// <summary>
    /// Вызывается на клиенте
    /// </summary>
    public void Hide();
}
