using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (CameraComponent))]
    [FriendOf(typeof (CameraComponent))]
    public static partial class CameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CameraComponent self)
        {
            self.cameraMode = CameraMode.Third;
            self.updateMode = CameraUpdateMode.Immediate;

            self.mainCamera = Global.Instance.MainCamera;
            self.mainCamera.backgroundColor = RenderSettings.fogColor;
            self.cameraRoot = Global.Instance.CameraRoot;
            var collector = self.cameraRoot.GetComponent<ReferenceCollector>();
            self.yawNode = collector.Get<Transform>("Yaw");
            self.cameraNode = collector.Get<Transform>("Camera");
            self.replaceNode = collector.Get<Transform>("Replace");
            self.shakeNode = collector.Get<Transform>("Shake");

            self.ApplyCfg(Global.Instance.CameraConfig.ThirdCfg);
        }

        [EntitySystem]
        private static void LateUpdate(this CameraComponent self)
        {
            self.FollowTarget();

            Vector3 realPos = self.CameraRealPos();
            if (self.currentfg.SmoothTime > 0)
            {
                self.cameraNode.localPosition =
                        Vector3.SmoothDamp(self.cameraNode.localPosition, realPos, ref self.smoothDamp, self.currentfg.SmoothTime);
            }
            else
            {
                self.cameraNode.localPosition = realPos;
            }

            self.cameraNode.LookAt(self.cameraRoot);
        }

        public static void Scale(this CameraComponent self, float delta)
        {
            switch (self.cameraMode)
            {
                case CameraMode.Third:
                    self.ScaleThird(delta);
                    break;
                case CameraMode.Free:
                    self.ScaleFree(delta);
                    break;
            }
        }

        public static void Pitch(this CameraComponent self, float delta)
        {
            self.pitch -= delta;
            self.pitch = Mathf.Clamp(self.pitch, self.currentfg.PitchMin, self.currentfg.PitchMax);
            self.ScaleFree(0f);
        }

        /// <summary>
        /// 旋转摄像机
        /// </summary>
        /// <param name="self"></param>
        /// <param name="delta">旋转的角度</param>
        public static void Yaw(this CameraComponent self, float delta)
        {
            self.yaw += delta;
            self.yawNode.localRotation = Quaternion.Euler(0, self.yaw, 0);
        }

        public static void Lock(this CameraComponent self, Transform target, float time = 0.2f)
        {
            if (self.lockTarget == target)
            {
                self.realFollowTargetTime = time;
                return;
            }

            Transform old = self.lockTarget;
            self.lockTarget = target;
            self.locked = true;

            if (target)
            {
                if (!old)
                {
                    self.cameraRoot.position = target.position;
                    self.realFollowTargetTime = 0;
                }
                else
                {
                    self.realFollowTargetTime = time;
                }
            }
            else
            {
                self.cameraRoot.position = Vector3.zero;
                self.realFollowTargetTime = time;
            }
        }

        public static void ChangeCfg(this CameraComponent self)
        {
            switch (self.currentfg.Mode)
            {
                case CameraMode.Third:
                    self.ApplyCfg(Global.Instance.CameraConfig.FreeCfg);
                    break;
                case CameraMode.Free:
                    self.ApplyCfg(Global.Instance.CameraConfig.ThirdCfg);
                    break;
            }
        }

        private static void ScaleFree(this CameraComponent self, float delta)
        {
            float mag = self.cameraNodePos.magnitude + delta;
            mag = Mathf.Clamp(mag, self.currentfg.Near.magnitude, self.currentfg.Far.magnitude);
            self.cameraNodePos = Quaternion.Euler(self.pitch, 0, 0) * Vector3.back * mag;
        }

        private static void ScaleThird(this CameraComponent self, float delta)
        {
            self.cameraNodePos += self.currentfg.Distance.normalized * (delta * Global.Instance.CameraConfig.ScaleTime);
            float sqrMag = self.cameraNodePos.sqrMagnitude;
            if (sqrMag < self.currentfg.Near.sqrMagnitude)
            {
                self.cameraNodePos = self.currentfg.Near;
            }
            else if (sqrMag > self.currentfg.Far.sqrMagnitude)
            {
                self.cameraNodePos = self.currentfg.Far;
            }
        }

        private static void FollowTarget(this CameraComponent self)
        {
            if (!self.locked || !self.lockTarget)
            {
                return;
            }

            if (!self.cameraRoot)
            {
                return;
            }

            switch (self.updateMode)
            {
                case CameraUpdateMode.Smooth:
                    self.cameraRoot.position = Vector3.SmoothDamp(self.cameraRoot.position, self.lockTarget.position, ref self.smoothDamp,
                        self.currentfg.SmoothTime);
                    break;
                case CameraUpdateMode.Immediate:
                    if (self.realFollowTargetTime > 0.001f)
                    {
                        // 镜头运动快一点,最后阻尼太慢了
                        Vector3 position = self.cameraRoot.position;
                        Vector3 lockPositon = self.lockTarget.position;
                        float s = 4 / MathF.Max((position - lockPositon).magnitude, 0.04f);
                        position = Vector3.SmoothDamp(position, lockPositon, ref self.smoothDamp,
                            self.realFollowTargetTime, Mathf.Infinity, Mathf.Max(1, s) * Time.deltaTime);
                        self.cameraRoot.position = position;
                    }
                    else
                    {
                        self.cameraRoot.position = self.lockTarget.position;
                    }

                    break;
            }
        }

        private static Vector3 CameraRealPos(this CameraComponent self)
        {
            switch (self.cameraMode)
            {
                case CameraMode.Third:
                    Vector3 dir = Vector3.down * (1 + self.mainCamera.nearClipPlane);
                    Ray ray = new Ray(self.yawNode.position + self.yawNode.rotation * self.cameraNodePos + Vector3.up, dir);
                    if (Physics.Raycast(ray, out RaycastHit hit, 1 + self.mainCamera.nearClipPlane, LayerHelper.LayerMap))
                    {
                        return self.cameraNodePos + Vector3.up * (self.mainCamera.nearClipPlane - ((ray.origin - hit.point).magnitude - 1));
                    }

                    break;
                case CameraMode.Free:
                    dir = self.yawNode.rotation * self.cameraNodePos;
                    ray = new Ray(self.cameraRoot.position, dir);
                    if (Physics.Raycast(ray, out hit, self.cameraNodePos.magnitude - 0.1f, LayerHelper.LayerMap))
                    {
                        Vector3 pos = hit.point + hit.normal * self.mainCamera.nearClipPlane;
                        return Quaternion.Euler(0, -self.yaw, 0) * (pos - ray.origin);
                    }

                    break;
            }

            return self.cameraNodePos;
        }

        private static void ApplyCfg(this CameraComponent self, CameraCfg cfg)
        {
            self.currentfg = cfg;
            self.mainCamera.nearClipPlane = cfg.NearClipPlane;
            self.mainCamera.farClipPlane = cfg.FarClipPlane;
            self.ResetCamera();
        }

        private static void ResetCamera(this CameraComponent self)
        {
            self.yaw = self.currentfg.Yaw;
            self.smoothDamp = Vector3.zero;
            self.cameraNodePos = self.currentfg.Best;

            self.pitch = self.currentfg.PitchBest;
            self.yawNode.rotation = Quaternion.Euler(0, self.yaw, 0);

            if (self.currentfg.SmoothTime <= 0f)
            {
                self.cameraNode.localPosition = self.cameraNodePos;
                self.cameraNode.LookAt(self.cameraRoot);
            }
        }
    }
}