using UnityEngine.UI;
namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    public class MRTKUGUIInputField : InputField
    {
        public int SelectionPosition
        {
            get => caretSelectPositionInternal;
            set => caretSelectPositionInternal = value;
        }
        public override void OnUpdateSelected(BaseEventData eventData) { }
    }
}
