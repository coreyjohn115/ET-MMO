using System.Collections.Generic;
using Lean.Touch;
using MongoDB.Bson;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (HotKeyDict))]
    [FriendOf(typeof (HotKeyItem))]
    public static partial class HotKeyDictSystem
    {
        [EntitySystem]
        private static void Awake(this HotKeyDict self)
        {
        }

        public static void InitKeyCode(this HotKeyDict self)
        {
            var c = self.AddChildWithId<HotKeyItem>(KeyCodeConst.NormalAttack);
            c.Keys = new List<KeyCode> { KeyCode.Space };

            c = self.AddChildWithId<HotKeyItem>(KeyCodeConst.Skill1);
            c.Keys = new List<KeyCode> { KeyCode.Alpha1 };

            c = self.AddChildWithId<HotKeyItem>(KeyCodeConst.Skill2);
            c.Keys = new List<KeyCode> { KeyCode.Alpha2 };

            c = self.AddChildWithId<HotKeyItem>(KeyCodeConst.Skill3);
            c.Keys = new List<KeyCode> { KeyCode.Alpha3 };

            c = self.AddChildWithId<HotKeyItem>(KeyCodeConst.Skill4);
            c.Keys = new List<KeyCode> { KeyCode.Alpha4 };

            c = self.AddChildWithId<HotKeyItem>(KeyCodeConst.Skill5);
            c.Keys = new List<KeyCode> { KeyCode.Alpha5 };

            c = self.AddChildWithId<HotKeyItem>(KeyCodeConst.Skill6);
            c.Keys = new List<KeyCode> { KeyCode.Alpha6 };
        }

        public static void AddRange(this HotKeyDict self, HotKeyDict dst)
        {
            self.ClearChild();
            foreach (HotKeyItem value in dst.Children.Values)
            {
                Log.Error(value.ToJson());
                self.AddChild(value);
            }
        }
    }

    [EntitySystemOf(typeof (PcInputComponent))]
    [FriendOf(typeof (PcInputComponent))]
    [FriendOf(typeof (HotKeyItem))]
    public static partial class PcInputComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PcInputComponent self)
        {
            HotKeyDict entity = self.Root().GetComponent<DataSaveComponent>().Get<HotKeyDict>(nameof (HotKeyDict));
            if (entity != default)
            {
                self.AddComponent(entity);
            }
            else
            {
                entity = self.AddComponent<HotKeyDict>();
                entity.InitKeyCode();
                self.Root().GetComponent<DataSaveComponent>().Save(nameof (HotKeyDict), entity);
            }

            foreach (var pair in entity.Children)
            {
                var item = pair.Value as HotKeyItem;
                self.hotKeyDict.Add(pair.Key, new HotKey() { Keys = item.Keys });
            }

            self.syncTime = 0.3f;
            self.pathfindingResult = M2C_PathfindingResult.Create();
        }

        [EntitySystem]
        private static void Update(this PcInputComponent self)
        {
            // Get the fingers we want to use
            var fingers = self.Use.GetFingers();

            //缩放
            var pinchScale = LeanGesture.GetPinchRatio(fingers, 0.1f);
            if (pinchScale != 1.0f)
            {
                self.Scene().GetComponent<CameraComponent>().Scale(pinchScale - 1f);
            }

            //切换视角
            if (Input.GetKeyDown(KeyCode.V))
            {
                self.Scene().GetComponent<CameraComponent>().ChangeCfg();
            }

            if (Input.anyKeyDown)
            {
                // foreach (KeyCode keyCode in System.Enum.GetValues(typeof (KeyCode)))
                // {
                //     if (Input.GetKeyDown(keyCode))
                //     {
                //         Log.Info("当前按下的按键类型：" + keyCode);
                //         break;
                //     }
                // }
            }
        }

        public static void AddHotKey(this PcInputComponent self, int key, params KeyCode[] keys)
        {
            if (self.hotKeyDict.TryGetValue(key, out HotKey hotKey))
            {
                return;
            }

            hotKey = new HotKey() { Keys = new List<KeyCode>() };
            foreach (KeyCode code in keys)
            {
                hotKey.Keys.Add(code);
            }

            self.hotKeyDict.Add(key, hotKey);
        }

        private static void CheckHotKey(this PcInputComponent self)
        {
            foreach (var pair in self.hotKeyDict)
            {
                if (pair.Value.Check())
                {
                    HotKeyItem data = self.GetComponent<HotKeyDict>().GetChild<HotKeyItem>(pair.Key);
                    EventSystem.Instance.Publish(self.Scene(), new HotKeyEvent() { KeyCode = pair.Key, HotEntity = data });
                }
            }
        }

        private static Vector3 AxisToSceneDir(float h, float v)
        {
            Vector3 moveDir = Vector3.zero;
            moveDir.Set(h, 0f, v);
            Vector3 vDir = Global.Instance.MainCamera.transform.rotation.eulerAngles;
            vDir.x = 0f;
            Quaternion qDir = Quaternion.Euler(vDir);
            moveDir = qDir * moveDir;
            moveDir.Normalize();
            return moveDir;
        }

        [EntitySystem]
        private static void LateUpdate(this PcInputComponent self)
        {
            self.CheckHotKey();
            if (Input.GetKey(KeyCode.Q))
            {
                self.Scene().GetComponent<CameraComponent>().Yaw(2f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                self.Scene().GetComponent<CameraComponent>().Yaw(-2f);
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool bMoving = (h != 0.0f) || (v != 0.0f);
            if (bMoving)
            {
                Unit myUnit = UnitHelper.GetMyUnitFromCurrentScene(self.Scene());
                if (!myUnit.CanMove())
                {
                    return;
                }

                Vector3 dir = AxisToSceneDir(h, v);
                myUnit.Forward = dir;

                if (!myUnit.GetComponent<MoveComponent>().IsArrived())
                {
                    return;
                }

                float speed = myUnit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Speed);
                var dst = myUnit.Position + myUnit.Forward * speed * self.syncTime;
                self.pathfindingResult.Points.Clear();
                myUnit.GetComponent<PathfindingComponent>().Find(myUnit.Position, dst, self.pathfindingResult.Points);

                if (self.pathfindingResult.Points.Count > 0)
                {
                    C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create(true);
                    c2MPathfindingResult.Position = dst;
                    self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
                    myUnit.GetComponent<MoveComponent>().MoveToAsync(self.pathfindingResult.Points, speed).NoContext();
                    self.moving = true;
                }
            }
            else if (self.moving)
            {
                self.moving = false;
                Unit myUnit = UnitHelper.GetMyUnitFromCurrentScene(self.Scene());
                myUnit.GetComponent<MoveComponent>().Stop(false);
                self.Root().GetComponent<ClientSenderComponent>().Send(C2M_Stop.Create());
            }
        }
    }
}