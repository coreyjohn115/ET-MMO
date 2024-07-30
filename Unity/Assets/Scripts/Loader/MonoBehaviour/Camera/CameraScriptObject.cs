using UnityEngine;

namespace ET
{
    [CreateAssetMenu(menuName = "ET/CreateCameraConfig", fileName = "CameraCfg", order = 1)]
    public class CameraScriptObject: ScriptableObject
    {
        public float ScaleTime = 6;
        public CameraCfg ThirdCfg;
        public CameraCfg FreeCfg;
    }
}