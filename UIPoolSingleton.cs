using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPoolSingleton
{
    private Dictionary<string, UIBasePanel> _pool  = new Dictionary<string, UIBasePanel>();

    static private UIPoolSingleton _instance;
    static public UIPoolSingleton Instance {get {  return _instance ?? new UIPoolSingleton();} }
    

    public bool Contain(IUIReq req)
    {
        return _pool.ContainsKey(req.Path);
    }

    public bool Contain(string path)
    {
        return _pool.ContainsKey(path);
    }

    public UIBasePanel FetchPanelFromPool(IUIReq req)
    {
        if (Contain(req))  return _pool[req.Path];
        throw new InvalidOperationException("UIPool doesn't contain it!");
    }

    public void AddUItoPool(IUIReq req)
    {
        this._pool[req.Path] = req.UI;
    }

    public void RemoveUIfromPool(IUIReq req)
    {
        if (!Contain(req)) return;
        this._pool.Remove(req.Path);
    }

    public void RemoveUIfromPool(string path)
    {
        if (!_pool.ContainsKey(path)) return;
        this._pool.Remove(path);
    }


	
}
