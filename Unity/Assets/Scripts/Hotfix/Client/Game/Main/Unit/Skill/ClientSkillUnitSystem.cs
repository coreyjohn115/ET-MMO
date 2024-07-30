namespace ET.Client
{
    [EntitySystemOf(typeof (ClientSkillUnit))]
    public static partial class ClientSkillUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ClientSkillUnit self)
        {
        }

        public static bool CheckValid(this ClientSkillUnit self)
        {
            return true;
        }
    }
}