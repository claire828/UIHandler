using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UIBasePanel : MonoBehaviour {

    public IUIReq Req { get; private set; }
    private object[] Datas { get;  set; }
    private bool InWaitingState { get; set; }
    private bool WaitingForFinish { get; set; }

    public void Initial(IUIReq req)
    {
        WaitingForFinish = false;
        Req = req;
    }

    #region override funcs If You need to manipulate data before Its Action
    public virtual IEnumerator ImplementUIBeforeAppearance()
    {
        yield break;
    }

    public virtual IEnumerator ImplementUIBeforeHide()
    {
        yield break;
    }

    public virtual IEnumerator ImplementUIBeforeDestroy()
    {
        yield break;
    }
    #endregion




    public IEnumerator Show()
    {
        yield return ImplementUIBeforeAppearance();
        gameObject.SetActive(true);

    }

    public IEnumerator Wait()
    {
        InWaitingState = true;
        WaitingForFinish = true;
        if (WaitingForFinish)
        {
            yield return null;
        }
        InWaitingState = false;
    }


    public IEnumerator Hide()
    {
        yield return ImplementUIBeforeHide();
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        ImplementUIBeforeDestroy();
    }

    public void Close()
    {
        if (InWaitingState)
        {
            this.WaitingForFinish = false;
        }
        else
        {
            UIHandlerSingleton.Instance.Hide(this);
        }  
    
    }




}
