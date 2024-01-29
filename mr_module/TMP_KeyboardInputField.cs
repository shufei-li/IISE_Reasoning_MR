using UnityEngine;
using UnityEngine.UI;
namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{   
    public override string Text { get => inputField.text; protected set => inputField.text = value; }
    protected override Graphic TextGraphic(MRTKTMPInputField inputField) => inputField.textComponent;
    protected override Graphic PlaceHolderGraphic(MRTKTMPInputField inputField) => inputField.placeholder;
    protected override void SyncCaret()
    {
        inputField.caretPosition = CaretIndex;
        inputField.SelectionPosition = CaretIndex;
    }
}

