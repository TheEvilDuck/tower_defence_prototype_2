using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParentsManager
{
    private List<IMenuParent>_menuParents;

    public MenuParentsManager()
    {
        _menuParents = new List<IMenuParent>();
    }

    public void Add(IMenuParent menuParent)
    {
        if (_menuParents.Contains(menuParent))
            throw new Exception("Menu parents shouldn't be duplicated in manager");

        _menuParents.Add(menuParent);
    }

    public void Show(IMenuParent menuParentToShow)
    {
        if (!_menuParents.Contains(menuParentToShow))
            throw new ArgumentException($"Did you forget to add {menuParentToShow}?", "menuName");

        foreach (IMenuParent menuParent in _menuParents)
            if (menuParent!=menuParentToShow)
                menuParent.Hide();

        if (menuParentToShow.Active)
            menuParentToShow.Hide();
        else
            menuParentToShow.Show();
    }
    public void Hide(IMenuParent menuParentToHide)
    {
        if (!_menuParents.Contains(menuParentToHide))
            throw new ArgumentException($"Did you forget to add {menuParentToHide}?", "menuName");

        menuParentToHide.Hide();
    }
}
