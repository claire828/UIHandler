using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUIReq 
{
    public string Path { get; }

    public bool IsUseable { get; }

    public object[] InitData { get; protected set; }

    public UIBasePanel UI { get; set; }



}



