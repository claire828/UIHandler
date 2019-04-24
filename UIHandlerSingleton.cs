using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandlerSingleton : MonoBehaviour
{
    #region Initial SingleTon

    static private UIHandlerSingleton _instance;
    static public UIHandlerSingleton Instance {get{ return _instance ?? BuildHandler();}}

    static private UIHandlerSingleton BuildHandler()
    {
         GameObject obj = new GameObject("UIPool", typeof(UIHandlerSingleton));
        DontDestroyOnLoad(obj);
        _instance = obj.GetComponent<UIHandlerSingleton>();
    }

    #endregion


    #region UI Action
  
    public IEnumerator CreateThenShow(GameObject parent, IUIReq req)
    {
        yield return Create(parent, req);
        Show(req);
    }

    public IEnumerator CreateThenShowWaiting(GameObject parent, IUIReq req)
    {
        yield return CreateThenShow(parent, req);
        yield return req.UI.Wait();
        yield return Hide(req.UI);
        Remove(req.UI);
    }


    private IEnumerator Create(GameObject parent, IUIReq req)
    {
        if (UIPoolSingleton.Instance.Contain(req))
        {
            req.UI = FetchUIFromPool(req);
        }
        else
        {
            ResourceRequest resource = Resources.LoadAsync(req.Path, typeof(GameObject));
            yield return resource;
            if (resource.asset == null) throw new InvalidCastException("Unable To Load UI: Action Failed");
            req.UI = FetchUIFromAsset(resource);
            req.UI.Initial(req);
            if (req.IsUseable) UIPoolSingleton.Instance.AddUItoPool(req);
        }
        req.UI.transform.SetParent(parent.transform, false);
       
    }



    public IEnumerator Hide(UIBasePanel panel)
    {
        yield return panel.Hide();
        Remove(panel);
    }


    private void Show(IUIReq req)
    {
        StartCoroutine(req.UI.Show());
    }


    private void Remove(UIBasePanel panel)
    {
        if (panel.Req.IsUseable)
        {
            panel.transform.SetParent(transform, false);
        }
        else
        {
            GameObject.Destroy(panel.gameObject);
        }
    }


  


    #endregion


    #region The Ways Of Fetching UI

    private UIBasePanel FetchUIFromAsset(ResourceRequest resource)
    {
        var child = GameObject.Instantiate(resource.asset) as GameObject;
        Transform trans = child.transform;
        return trans.GetComponent<UIBasePanel>();
    }


    private UIBasePanel FetchUIFromPool(IUIReq req)
    {
        return UIPoolSingleton.Instance.FetchPanelFromPool(req);
    }

    #endregion

 


}
