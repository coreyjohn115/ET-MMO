namespace ET.Client
{
    public enum Ease
    {
        Unset,
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce,
        Flash,
        InFlash,
        OutFlash,
        InOutFlash,

        /// <summary>
        /// Don't assign this! It's assigned automatically when creating 0 duration tweens
        /// </summary>
        INTERNAL_Zero,

        /// <summary>
        /// Don't assign this! It's assigned automatically when setting the ease to an AnimationCurve
        /// or to a custom ease function
        /// </summary>
        INTERNAL_Custom,
    }
}