using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class UnitWeaponComponent: Entity, IAwake, IDestroy
    {
        public GameObject go;
    }
}