using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FungusExtra { 

    /// <summary>
    /// Input handler for say dialogs.
    /// </summary>
    public class DialogInputExtra : MonoBehaviour
    {
        [Tooltip("クリックして記事を進める")]
        [SerializeField] protected ClickMode clickMode;

        [Tooltip("連続してクリックする際の遅延時間。誤ってストーリーをクリックしてしまうのを防ぐのに有効です。 ←これバグあるよ")]
        [SerializeField] protected float nextClickDelay = 0f;

        [Tooltip("キャンセルを押しながらの早送りが可能")]
        [SerializeField] protected bool cancelEnabled = true;

        [Tooltip("メニューダイアログがアクティブな場合、入力を無視する")]
        [SerializeField] protected bool ignoreMenuClicks = true;


        [Tooltip("連続してクリックする際の遅延時間。")]
        [SerializeField] protected float nextClickDelay_ClickAnywhere = 0.1f;

        protected bool dialogClickedFlag;

        protected bool nextLineInputFlag;

        protected float ignoreClickTimer;
        protected float ignoreClickTimerClickAnywhere;

        protected StandaloneInputModule currentStandaloneInputModule;

        protected Writer writer;


        [SerializeField]
        Toggle m_Toggle;

        [SerializeField]
        Toggle m_OptionMenu;

        [SerializeField, ReadOnly] private bool m_fastforward           = false;
        [SerializeField, ReadOnly] private bool m_forceignore           = false;

        public void SetFastForward() {
            m_fastforward = m_Toggle.isOn;
        }
        public void ToggleIgnore(bool t)
        {
            m_forceignore = t;
        }

        protected virtual void Awake()
        {
            writer = GetComponent<Writer>();
            CheckEventSystem();

            // ディレイタイム初期化
            ignoreClickTimerClickAnywhere = nextClickDelay_ClickAnywhere;
        }

        // SayとMenuの入力が機能するためには、シーンにイベントシステムが存在しなければなりません。
        // 存在しない場合、このメソッドは自動的にインスタンスを作成します。
        protected virtual void CheckEventSystem()
        {
            EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                // Auto spawn an Event System from the prefab
                GameObject prefab = Resources.Load<GameObject>("Prefabs/EventSystem");
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab) as GameObject;
                    go.name = "EventSystem";
                }
            }
        }

        protected virtual void Update()
        {
            if (EventSystem.current == null)
            {
                return;
            }

            if (currentStandaloneInputModule == null)
            {
                currentStandaloneInputModule = EventSystem.current.GetComponent<StandaloneInputModule>();
            }

            // 早送り
            if (writer != null && writer.IsWriting)
            {
                if (Input.GetButtonDown(currentStandaloneInputModule.submitButton) ||
                    (cancelEnabled && Input.GetButton(currentStandaloneInputModule.cancelButton))
                    || m_fastforward )
                {
                    SetNextLineFlag();
                }
            }

            if (ignoreMenuClicks)
            {
                // メニューが表示されている場合、入力イベントを無視する
                if (MenuDialog.ActiveMenuDialog != null &&
                    MenuDialog.ActiveMenuDialog.IsActive() &&
                    MenuDialog.ActiveMenuDialog.DisplayedOptionsCount > 0)
                {
                    dialogClickedFlag = false;
                    nextLineInputFlag = false;
                }
            }

            if (m_forceignore)
            {
                return;
            }

            switch (clickMode)
            {
                case ClickMode.Disabled:
                    break;
                case ClickMode.ClickAnywhere:

                    if (writer.IsWriting && Input.GetMouseButtonDown(0) && ignoreClickTimerClickAnywhere <= 0.0f)
                    {
                        SetNextLineFlag();
                        ignoreClickTimerClickAnywhere = nextClickDelay_ClickAnywhere;
                    }
                    break;
                case ClickMode.ClickOnDialog:
                    if (dialogClickedFlag)
                    {
                        SetNextLineFlag();
                        dialogClickedFlag = false;
                    }
                    break;
            }

            if (ignoreClickTimer > 0f)
            {
                ignoreClickTimer = Mathf.Max(ignoreClickTimer - Time.deltaTime, 0f);
            }
            if (nextClickDelay_ClickAnywhere > 0f)
            {
                ignoreClickTimerClickAnywhere = Mathf.Max(ignoreClickTimerClickAnywhere - Time.deltaTime, 0f);
            }



            // 次の行に移動するようにリスナーに伝える
            if (nextLineInputFlag)
            {
                var inputListeners = gameObject.GetComponentsInChildren<IDialogInputListener>();
                for (int i = 0; i < inputListeners.Length; i++)
                {
                    var inputListener = inputListeners[i];
                    inputListener.OnNextLineEvent();
                }
                nextLineInputFlag = false;
            }
        }

        #region Public members

        /// <summary>
        /// スクリプトから次の行の入力イベントをトリガーする。
        /// </summary>
        public virtual void SetNextLineFlag()
        {
            nextLineInputFlag = true;
        }

        /// <summary>
        /// ダイアログがクリックされたフラグを設定します（通常は、ダイアログUIのイベントトリガーコンポーネントから）。
        /// </summary>
        public virtual void SetDialogClickedFlag()
        {
            // 繰り返しのクリックを短時間無視することで、誤ってキャラクターの台詞をクリックしてしまうことを防ぎます。
            if (ignoreClickTimer > 0f)
            {
                return;
            }
            ignoreClickTimer = nextClickDelay;

            // クリックオンダイアログモードでのみ適用
            if (clickMode == ClickMode.ClickOnDialog)
            {
                dialogClickedFlag = true;
            }
        }

        /// <summary>
        /// ボタンがクリックされたフラグを設定します。
        /// </summary>
        public virtual void SetButtonClickedFlag()
        {
            // クリックが無効になっていない場合のみ適用
            if (clickMode != ClickMode.Disabled)
            {
                SetNextLineFlag();
            }
        }

        #endregion
    }
}