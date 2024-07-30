using UnityEngine;

namespace ET
{
    [CreateAssetMenu(menuName = "ET/GameScriptObject", fileName = "GameCfg", order = 1)]
    public class GameScriptObject: ScriptableObject
    {
        public int UICacheTimer = 5;
    }
}