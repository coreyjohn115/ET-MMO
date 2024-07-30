using System.Collections.Generic;
using Lean.Touch;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    public struct HotKeyEvent
    {
        public long KeyCode;
        public HotKeyItem HotEntity;
    }

    public struct HotKey
    {
        public List<KeyCode> Keys;
        
        public bool Check()
        {
            bool hasDown = false;
            for (int i = 0; i < this.Keys.Count; i++)
            {
                KeyCode code = this.Keys[i];
                if (!Input.GetKeyDown(code))
                {
                    continue;
                }

                hasDown = true;
                break;
            }

            if (!hasDown)
            {
                return false;
            }

            for (int i = 0; i < this.Keys.Count; i++)
            {
                KeyCode code = this.Keys[i];
                if (!Input.GetKey(code))
                {
                    return false;
                }
            }

            return true;
        }
    }

    [ChildOf(typeof (HotKeyDict))]
    public class HotKeyItem: Entity, IAwake, ISerializeToEntity
    {
        public List<KeyCode> Keys;
        public int Desc;
    }

    /// <summary>
    /// 保存游戏快捷键
    /// </summary>
    [ComponentOf(typeof (PcInputComponent))]
    public class HotKeyDict: Entity, IAwake
    {
        
    }

    [ComponentOf(typeof (Scene))]
    public class PcInputComponent: Entity, IAwake, IUpdate, ILateUpdate
    {
        public LeanFingerFilter Use = new(true);

        public Dictionary<long, HotKey> hotKeyDict = new();

        public M2C_PathfindingResult pathfindingResult;
        public float3 lastCheck;
        public float syncTime;
        public bool moving;
    }
}