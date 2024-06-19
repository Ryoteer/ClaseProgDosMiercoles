using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    #region Singleton
    public static UpdateManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private List<IUpdate> _updateableObjs = new();

    public void AddObject(IUpdate obj)
    {
        _updateableObjs.Add(obj);
    }

    public void RemoveObject(IUpdate obj) 
    {
        if(_updateableObjs.Contains(obj)) _updateableObjs.Remove(obj);
    }

    public void Clear()
    {
        _updateableObjs.Clear();
    }

    private void Update()
    {
        foreach(var obj in _updateableObjs)
        {
            obj.ArtUpdate();
        }
    }

    private void FixedUpdate()
    {
        foreach (var obj in _updateableObjs)
        {
            obj.ArtFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        foreach (var obj in _updateableObjs)
        {
            obj.ArtLateUpdate();
        }
    }
}
