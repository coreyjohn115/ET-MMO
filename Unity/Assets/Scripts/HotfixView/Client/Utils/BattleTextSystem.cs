using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof (BattleText))]
    [FriendOf(typeof (BattleText))]
    public static partial class BattleTextSystem
    {
        [EntitySystem]
        private static void Awake(this BattleText self)
        {
            self.timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(100, TimerInvokeType.BattleText, self);
        }

        [EntitySystem]
        private static void Destroy(this BattleText self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.timer);
            foreach (string s in self.hudNameSet)
            {
                GameObjectPoolHelper.ClearPool(s);
            }

            self.hudNameSet.Clear();
            self.waitPopDict.Clear();
        }

        [Invoke(TimerInvokeType.BattleText)]
        public class BattleTextTimer: ATimer<BattleText>
        {
            protected override void Run(BattleText self)
            {
                self.OnUpdate();
            }
        }

        private static void OnUpdate(this BattleText self)
        {
            if (self.waitPopDict.Count == 0)
            {
                return;
            }

            using ListComponent<long> dels = ListComponent<long>.Create();
            foreach ((long dstId, List<Pair<long, string>> list) in self.waitPopDict)
            {
                long id = list[0].Key;
                string name = list[0].Value;
                self.PlayHud(id, name);
                list.RemoveAt(0);
                if (list.Count == 0)
                {
                    dels.Add(dstId);
                }
            }

            foreach (long l in dels)
            {
                self.waitPopDict.Remove(l);
            }
        }

        private static string GetHudName(long id, HurtProto proto)
        {
            if (proto.IsAddHp)
            {
                return "ES_Hp";
            }

            if (id == proto.Id)
            {
                if (proto.IsImmUnity || proto.IsFender)
                {
                    return "ES_SelfStatusTip";
                }

                if (proto.IsCrit)
                {
                    return "ES_SelfHurtCrit";
                }

                return "ES_SelfHurt";
            }

            if (proto.IsImmUnity || proto.IsFender)
            {
                return "ES_OtherStatusTip";
            }

            if (proto.IsCrit)
            {
                return "ES_OtherHurtCrit";
            }

            return "ES_OtherHurt";
        }

        private static void PlayHud(this BattleText self, long id, string name)
        {
            switch (name)
            {
                case "ES_Hp":
                    var hp = self.GetChild<ES_Hp>(id);
                    break;
                case "ES_SelfStatusTip":
                    var selfTip = self.GetChild<ES_SelfStatusTip>(id);
                    break;
                case "ES_SelfHurtCrit":
                    var selfCrit = self.GetChild<ES_SelfHurtCrit>(id);
                    break;
                case "ES_SelfHurt":
                    ES_SelfHurt selfHurt = self.GetChild<ES_SelfHurt>(id);
                    selfHurt.Play();
                    break;
                case "ES_OtherStatusTip":
                    var otherTip = self.GetChild<ES_OtherStatusTip>(id);
                    break;
                case "ES_OtherHurtCrit":
                    var otherCrit = self.GetChild<ES_OtherHurtCrit>(id);
                    break;
                case "ES_OtherHurt":
                    ES_OtherHurt otherHurt = self.GetChild<ES_OtherHurt>(id);
                    otherHurt.Play();
                    break;
            }
        }

        private static Entity AddESEntity(this BattleText self, string name, long caster, GameObject go, HurtProto proto)
        {
            switch (name)
            {
                case "ES_Hp":
                    var hp = self.AddChild<ES_Hp, Transform>(go.transform, true);
                    return hp;
                case "ES_SelfStatusTip":
                    var selfTip = self.AddChild<ES_SelfStatusTip, Transform>(go.transform, true);
                    return selfTip;
                case "ES_SelfHurtCrit":
                    var selfCrit = self.AddChild<ES_SelfHurtCrit, Transform>(go.transform, true);
                    return selfCrit;
                case "ES_SelfHurt":
                    var selfHurt = self.AddChild<ES_SelfHurt, Transform>(go.transform, true);
                    selfHurt.Initliaze(caster, proto);
                    return selfHurt;
                case "ES_OtherStatusTip":
                    var otherTip = self.AddChild<ES_OtherStatusTip, Transform>(go.transform, true);
                    return otherTip;
                case "ES_OtherHurtCrit":
                    var otherCrit = self.AddChild<ES_OtherHurtCrit, Transform>(go.transform, true);
                    return otherCrit;
                case "ES_OtherHurt":
                    var otherHurt = self.AddChild<ES_OtherHurt, Transform>(go.transform, true);
                    otherHurt.Initliaze(caster, proto);
                    return otherHurt;
            }

            return default;
        }

        public static async ETTask PopHud(this BattleText self, long id, Unit dst, HurtProto proto)
        {
            string hudName = GetHudName(id, proto);
            long oldId = dst.Id;
            string path = hudName.ToUIHudPath();
            GameObject prefab = await self.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            if (oldId != dst.Id)
            {
                return;
            }

            if (self.hudNameSet.Add(path))
            {
                GameObjectPoolHelper.InitPool(hudName, prefab, 3, PoolInflationType.INCREMENT);
            }

            GameObject go = GameObjectPoolHelper.GetObjectFromPool(hudName);
            Entity hud = self.AddESEntity(hudName, id, go, proto);
            if (!self.waitPopDict.TryGetValue(dst.Id, out var list))
            {
                list = new List<Pair<long, string>>();
                self.waitPopDict.Add(dst.Id, list);
            }

            list.Add(Pair<long, string>.Create(hud.Id, hudName));
        }
    }
}