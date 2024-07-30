using System;
using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// Unit的组件名称
/// </summary>
[Code]
public class UnitComponentSingleton: Singleton<UnitComponentSingleton>, ISingletonAwake
{
    private readonly Dictionary<string, string> unitComDict = [];

    public void Awake()
    {
        foreach (Type v in CodeTypes.Instance.GetTypes(typeof (UnitComAttribute)))
        {
            this.unitComDict.Add(v.Name, v.FullName);
        }
    }

    public Dictionary<string, string> GetUnitComs()
    {
        return this.unitComDict;
    }
}