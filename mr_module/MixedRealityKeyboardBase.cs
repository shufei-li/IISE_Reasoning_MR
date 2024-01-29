using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using UnityEngine.Events;
#if WINDOWS_UWP
using Windows.Globalization;
using Windows.UI.ViewManagement;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
#endif
namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    public abstract class MixedRealityKeyboardBase : MonoBehaviour
    {
        #region Properties
        public bool Visible => state == KeyboardState.Showing;
        public int CaretIndex
        {
            get;
            private set;
        } = 0;
        [Experimental, SerializeField, Tooltip("Whether disable user's interaction with other UI elements while typing. Use this option to decrease the chance of keyboard getting accidentally closed.")]
        private bool disableUIInteractionWhenTyping = false;
        public bool DisableUIInteractionWhenTyping
        {
            get => disableUIInteractionWhenTyping;
            set
            {
                if (value != disableUIInteractionWhenTyping && value == false && inputModule != null && inputModule.ProcessPaused)
                {
                    inputModule.ProcessPaused = false;
                }
                disableUIInteractionWhenTyping = value;
            }
        }
        [SerializeField, Tooltip("Event which triggers when the keyboard is shown.")]
        private UnityEvent onShowKeyboard = new UnityEvent();

        public UnityEvent OnShowKeyboard
        {
            get => onShowKeyboard;
            set => onShowKeyboard = value;
        }
        [SerializeField, Tooltip("Event which triggers when commit action is invoked on the keyboard. (Usually the return key.)")]
        private UnityEvent onCommitText = new UnityEvent();

        public UnityEvent OnCommitText
        {
            get => onCommitText;
            set => onCommitText = value;
        }
        [SerializeField, Tooltip("Event which triggers when the keyboard is hidden.")]
        private UnityEvent onHideKeyboard = new UnityEvent();
        public UnityEvent OnHideKeyboard
        {
            get => onHideKeyboard;
            set => onHideKeyboard = value;
        }
        private enum KeyboardState
        {
            Hiding,
            Hidden,
            Showing,
        }
        private KeyboardState state = KeyboardState.Hidden;
        private bool multiLine = false;
        private MixedRealityInputModule inputModule = null;
        private InputPane inputPane = null;
        private TouchScreenKeyboard keyboard = null;
        private Coroutine stateUpdate;
        private string keyboardLanguage = string.Empty;
        protected virtual void Awake()
        {
            inputModule = CameraCache.Main.GetComponent<MixedRealityInputModule>();
        }
        protected virtual void Start()
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                inputPane = InputPane.GetForCurrentView();
                inputPane.Hiding += OnInputPaneHiding;
                inputPane.Showing += OnInputPaneShowing;
            }, false);
        }
        private void OnInputPaneHiding(InputPane inputPane, InputPaneVisibilityEventArgs args)
        {
            OnKeyboardHiding();
            if (DisableUIInteractionWhenTyping && inputModule != null)
            {
                inputModule.ProcessPaused = false;
            }
        }
        private void OnInputPaneShowing(InputPane inputPane, InputPaneVisibilityEventArgs args)
        {
            OnKeyboardShowing();
            if (DisableUIInteractionWhenTyping && inputModule != null)
            {
                inputModule.ProcessPaused = true;
            }
        }
        void OnDestroy()
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                inputPane = InputPane.GetForCurrentView();
                inputPane.Hiding -= OnInputPaneHiding;
                inputPane.Showing -= OnInputPaneShowing;
            }, false);
        }
        private IEnumerator UpdateState()
        {
            while (true)
            {
                switch (state)
                {
                    case KeyboardState.Showing:
                        {
                            UpdateText();
                        }
                        break;
                }
                yield return null;
            }
        }
        private void OnDisable()
        {
            HideKeyboard();
        }
        public abstract string Text { get; protected set; }
        public void HideKeyboard()
        {
            if (state != KeyboardState.Hidden)
            {
                state = KeyboardState.Hidden;
            }
            UnityEngine.WSA.Application.InvokeOnUIThread(() => inputPane?.TryHide(), false);
            if (stateUpdate != null)
            {
                StopCoroutine(stateUpdate);
                stateUpdate = null;
            }
        }
        public virtual void ShowKeyboard(string text = "", bool multiLine = false)
        {
            Text = text;
            this.multiLine = multiLine;
            state = KeyboardState.Showing;
            if (keyboard != null)
            {
                keyboard.text = Text;
                UnityEngine.WSA.Application.InvokeOnUIThread(() => inputPane?.TryShow(), false);
            }
            else
            {
                keyboard = TouchScreenKeyboard.Open(Text, TouchScreenKeyboardType.Default, false, this.multiLine, false, false);
            }
            onShowKeyboard?.Invoke();
            keyboard.selection = new RangeInt(Text.Length, 0);
            MovePreviewCaretToEnd();
            if (stateUpdate == null)
            {
                stateUpdate = StartCoroutine(UpdateState());
            }
        }
        public virtual void ClearKeyboardText()
        {
            Text = string.Empty;
            CaretIndex = 0;
            if (keyboard != null)
            {
                keyboard.text = string.Empty;
            }
        }
        private void UpdateText()
        {
            if (keyboard != null)
            {
                Text = keyboard.text;
                CaretIndex = keyboard.selection.end;
                // Check the current language of the keyboard
                string newKeyboardLanguage = Language.CurrentInputMethodLanguageTag;
                if (newKeyboardLanguage != keyboardLanguage)
                {
                    keyboard.text = Text;
                    if (IsIMERequired(newKeyboardLanguage))
                    {
                        MovePreviewCaretToEnd();
                    }
                }
                keyboardLanguage = newKeyboardLanguage;
                var characterDelta = keyboard.text.Length - Text.Length;
                if (UnityEngine.Input.GetKey(KeyCode.Backspace) ||
                    UnityEngine.Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (Text.Length > keyboard.text.Length && IsIMERequired(keyboardLanguage))
                    {
                        Text = keyboard.text;
                        CaretIndex = Mathf.Clamp(CaretIndex + characterDelta, 0, Text.Length);
                    }
                    else if (CaretIndex > 0)
                    {
                        Text = Text.Remove(CaretIndex - 1, 1);
                        keyboard.text = Text;
                        --CaretIndex;
                    }
                }
                else if (IsIMERequired(keyboardLanguage))
                {
                    Text = keyboard.text;
                    MovePreviewCaretToEnd();
                }
                else
                {
                    var caretWasAtEnd = IsPreviewCaretAtEnd();
                    if (characterDelta > 0)
                    {
                        var newCharacters = keyboard.text.Substring(Text.Length, characterDelta);
                        Text = Text.Insert(CaretIndex, newCharacters);
                        if (keyboard.text != Text)
                        {
                            keyboard.text = Text;
                        }
                        if (caretWasAtEnd)
                        {
                            MovePreviewCaretToEnd();
                        }
                        else
                        {
                            CaretIndex += newCharacters.Length;
                        }
                    }
                    if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow) ||
                        UnityEngine.Input.GetKey(KeyCode.LeftArrow))
                    {
                        CaretIndex = Mathf.Clamp(CaretIndex - 1, 0, Text.Length);
                    }
                    if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow) ||
                        UnityEngine.Input.GetKey(KeyCode.RightArrow))
                    {
                        CaretIndex = Mathf.Clamp(CaretIndex + 1, 0, Text.Length);
                    }
                }
                if (!multiLine)
                {
                    if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
                    {
                        onCommitText?.Invoke();
                        HideKeyboard();
                    }
                }
                SyncCaret();
            }
        }
        private bool IsPreviewCaretAtEnd() => CaretIndex == Text.Length;
        private void MovePreviewCaretToEnd() => CaretIndex = Text.Length;
        private void OnKeyboardHiding()
        {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => onHideKeyboard?.Invoke(), false);
            state = KeyboardState.Hidden;
        }
        private void OnKeyboardShowing() { }
        private bool IsIMERequired(string language)
        {
            return language.StartsWith("zh") || language.StartsWith("ja") || language.StartsWith("ko");
        }
        protected virtual void SyncCaret() { }

    }
}
