using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (ActionUnit))]
    public static partial class ActionUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ActionUnit self, string name, float duration)
        {
            self.tcs = ETTask.Create(true);
            self.ActionName = name;
            self.outDuration = duration;
            self.index = 0;
            self.action = ActionSingleton.Instance.GetAction(self.Config.ActionType);
        }

        [EntitySystem]
        private static void Destroy(this ActionUnit self)
        {
            if (self.state < ActionState.Complete)
            {
                self.state = ActionState.Complete;
                self.UnExecute();
            }

            self.state = ActionState.Ready;
            self.duration = 0f;
            self.startTime = 0f;
            self.action = null;
        }

        public static void Finish(this ActionUnit self)
        {
            if (self.state == ActionState.Run)
            {
                self.state = ActionState.Complete;
                self.UnExecute();
            }
        }

        public static async ETTask WaitFinishAsync(this ActionUnit self)
        {
            await self.tcs;
        }

        public static void Update(this ActionUnit self)
        {
            if (self.action == null)
            {
                return;
            }

            Unit unit = self.Parent.GetParent<Unit>();
            switch (self.state)
            {
                case ActionState.Run:
                    self.action.OnUpdate(unit, self);
                    self.duration += Time.deltaTime;
                    if (self.Interval > 0 && self.duration >= self.Interval + self.Config.StartTime)
                    {
                        self.state = ActionState.Complete;
                        self.UnExecute();
                    }

                    break;
                case ActionState.Ready:
                    self.duration += Time.deltaTime;
                    if (self.duration >= self.Config.StartTime)
                    {
                        self.Execute();
                    }

                    break;
            }
        }

        private static void Execute(this ActionUnit self)
        {
            if (self.action == null)
            {
                return;
            }

            if (self.state != ActionState.Ready)
            {
                return;
            }

            self.startTime = Time.time;
            Unit unit = self.Parent.GetParent<Unit>();
            self.action.Execute(unit, self);
            self.state = ActionState.Run;
            self.action.OnExecute(unit, self).NoContext();
        }

        private static void UnExecute(this ActionUnit self)
        {
            if (self.action == null)
            {
                return;
            }

            if (self.state != ActionState.Complete)
            {
                return;
            }

            try
            {
                Unit unit = self.Parent.GetParent<Unit>();
                self.action.OnUnExecute(unit, self);
                self.action.OnFinish(unit, self);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                self.state = ActionState.Finish;
                self.tcs.SetResult();
            }
        }
    }
}