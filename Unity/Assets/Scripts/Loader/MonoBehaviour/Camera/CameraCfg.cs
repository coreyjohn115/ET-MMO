using System;
using UnityEngine;

namespace ET
{
    public enum CameraUpdateMode
    {
        Smooth,
        Immediate,
    }

    public enum CameraMode
    {
        Third,
        Free,
    }

    [Serializable]
    public class CameraCfg
    {
        public CameraMode Mode;
        public CameraUpdateMode UpdateMode;

        public float SmoothTime;

        public float NearClipPlane = 1;
        public float FarClipPlane = 500;

        public Vector3 Near;
        public Vector3 Far;
        public float BestRatio = 0.5f;

        public Vector3 Distance
        {
            get
            {
                return this.Far - this.Near;
            }
        }

        public Vector3 Best
        {
            get
            {
                switch (Mode)
                {
                    case CameraMode.Third:
                        return this.Near + (this.Far - this.Near) * this.BestRatio;
                    default:
                        return Quaternion.Euler(this.PitchBest, 0, 0) *
                                (Vector3.back * (this.Near + (this.Far - this.Near) * this.BestRatio).magnitude);
                }
            }
        }

        public float Yaw;
        public bool YawAtThird;

        public float PitchBest;
        public float PitchMin;
        public float PitchMax;
    }
}