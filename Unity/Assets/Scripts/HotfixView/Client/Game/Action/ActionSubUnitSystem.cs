using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (ActionSubUnit))]
    public static partial class ActionSubUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ActionSubUnit self, string name, float duration, ActionConfig config)
        {
            self.ActionName = name;
            self.outDuration = duration;
            self.Config = config;
            self.action = ActionSingleton.Instance.GetAction(self.Config.ActionType);
        }

        [EntitySystem]
        private static void Destroy(this ActionSubUnit self)
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

        public static void Finish(this ActionSubUnit self)
        {
            if (self.state == ActionState.Run)
            {
                self.state = ActionState.Complete;
                self.UnExecute();
            }
        }

        private static Unit GetUnit(this ActionSubUnit self)
        {
            return self.Parent.Parent.GetParent<Unit>();
        }

        public static void Update(this ActionSubUnit self)
        {
            if (self.state == ActionState.Finish || self.action == null)
            {
                return;
            }

            switch (self.state)
            {
                case ActionState.Run:
                    self.action.OnUpdate(self.GetUnit(), self);
                    self.duration += Time.deltaTime;
                    if (self.Interval > 0f && self.duration >= self.Interval + self.Config.StartTime)
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

        private static void Execute(this ActionSubUnit self)
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
            self.action.Execute(self.GetUnit(), self);
            self.state = ActionState.Run;
            self.action.OnExecute(self.GetUnit(), self).NoContext();
        }

        private static void UnExecute(this ActionSubUnit self)
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
                self.action.OnUnExecute(self.GetUnit(), self);
                self.action.OnFinish(self.GetUnit(), self);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                self.state = ActionState.Finish;
            }
        }
    }
}