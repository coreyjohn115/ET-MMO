using UnityEngine;

namespace ET.Client
{
    public static class UIFindHelper
    {
        /// <summary>
        /// 查找子节点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform FindDeepChild(GameObject target, string childName)
        {
            Transform resultTrs = target.transform.Find(childName);
            if (resultTrs == null)
            {
                foreach (Transform trs in target.transform)
                {
                    resultTrs = FindDeepChild(trs.gameObject, childName);
                    if (resultTrs != null)
                    {
                        return resultTrs;
                    }
                }
            }

            return resultTrs;
        }

        /// <summary>
        /// 根据泛型查找子节点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="childName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindDeepChild<T>(GameObject target, string childName) where T : Component
        {
            Transform resultTrs = FindDeepChild(target, childName);
            if (resultTrs != null)
            {
                return resultTrs.gameObject.GetComponent<T>();
            }

            return null;
        }
    }
}