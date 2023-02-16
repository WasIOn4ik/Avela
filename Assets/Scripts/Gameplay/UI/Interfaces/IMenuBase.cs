using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuBase
{
    /// <summary>
    /// ��� ����� ���������� �� �������, ��� ��� ���������� �� ������� �� �������
    /// </summary>
    public void Show(bool bHideOthers);

    /// <summary>
    /// ��� ����� ���������� �� �������, ��� ��� ���������� �� ������� �� �������
    /// </summary>
    public void ShowSubmenu(bool bHideOthers);

    /// <summary>
    /// ���������� �� �������
    /// </summary>
    public void Hide();
}
