using UnityEngine;
namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    [AddComponentMenu("Scripts/MRTK/Experimental/Keyboard/MixedRealityKeyboard")]
    public class MixedRealityKeyboard : MixedRealityKeyboardBase
    {
        public override string Text
        {
            get;
            protected set;
        } = string.Empty;
    }
}
