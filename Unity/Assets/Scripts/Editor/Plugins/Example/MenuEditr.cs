using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public static class MenuEditr
    {
        [MenuItem("ET/Tools/CreateBone _&#r")]
        public static void CreateUnitBone()
        {
            Transform aT = Selection.activeTransform;
            if (!aT)
            {
                return;
            }

            var boneList = new[] { UnitBone.Hud, UnitBone.Body, UnitBone.Chest, UnitBone.Down };
            var bNode = aT.Find("Bones");
            if (!bNode)
            {
                GameObject go = new GameObject("Bones");
                go.transform.SetParent(aT);
                bNode = go.transform;
            }

            var collect = aT.GetComponent<ReferenceCollector>();
            if (!collect)
            {
                collect = aT.gameObject.AddComponent<ReferenceCollector>();
            }

            collect.Clear();
            foreach (string bone in boneList)
            {
                Transform n = bNode.Find(bone);
                if (!n)
                {
                    GameObject go = new GameObject(bone);
                    go.transform.SetParent(bNode);
                    n = go.transform;
                }

                collect.Add(bone, n);
            }

            var audio = aT.GetComponentInChildren<AudioSource>();
            collect.Add("AudioSource", audio);

            var animator = aT.GetComponentInChildren<Animator>();
            if (animator)
            {
                collect.Add("Animator", animator);
            }

            var animation = aT.GetComponentInChildren<Animator>();
            if (animation)
            {
                collect.Add("Animation", animation);
            }

            EditorUtility.SetDirty(aT);
        }
    }
}