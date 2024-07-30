using Lean.Touch;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class CameraComponent: Entity, IAwake, ILateUpdate
    {
        public Camera mainCamera;
        public CameraUpdateMode updateMode;
        public CameraMode cameraMode;

        public Transform lockTarget;
        public bool locked;

        public Transform cameraRoot;
        public Vector3 smoothDamp;
        public float realFollowTargetTime;

        public Transform yawNode;
        public float yaw;
        public float pitch;

        public Vector3 cameraNodePos;
        public Transform shakeNode;
        public Transform replaceNode;
        public Transform cameraNode;

        public CameraCfg currentfg;
    }
}