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
        [Tooltip("�N���b�N���ċL����i�߂�")]
        [SerializeField] protected ClickMode clickMode;

        [Tooltip("�A�����ăN���b�N����ۂ̒x�����ԁB����ăX�g�[���[���N���b�N���Ă��܂��̂�h���̂ɗL���ł��B ������o�O�����")]
        [SerializeField] protected float nextClickDelay = 0f;

        [Tooltip("�L�����Z���������Ȃ���̑����肪�\")]
        [SerializeField] protected bool cancelEnabled = true;

        [Tooltip("���j���[�_�C�A���O���A�N�e�B�u�ȏꍇ�A���͂𖳎�����")]
        [SerializeField] protected bool ignoreMenuClicks = true;


        [Tooltip("�A�����ăN���b�N����ۂ̒x�����ԁB")]
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

            // �f�B���C�^�C��������
            ignoreClickTimerClickAnywhere = nextClickDelay_ClickAnywhere;
        }

        // Say��Menu�̓��͂��@�\���邽�߂ɂ́A�V�[���ɃC�x���g�V�X�e�������݂��Ȃ���΂Ȃ�܂���B
        // ���݂��Ȃ��ꍇ�A���̃��\�b�h�͎����I�ɃC���X�^���X���쐬���܂��B
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

            // ������
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
                // ���j���[���\������Ă���ꍇ�A���̓C�x���g�𖳎�����
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



            // ���̍s�Ɉړ�����悤�Ƀ��X�i�[�ɓ`����
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
        /// �X�N���v�g���玟�̍s�̓��̓C�x���g���g���K�[����B
        /// </summary>
        public virtual void SetNextLineFlag()
        {
            nextLineInputFlag = true;
        }

        /// <summary>
        /// �_�C�A���O���N���b�N���ꂽ�t���O��ݒ肵�܂��i�ʏ�́A�_�C�A���OUI�̃C�x���g�g���K�[�R���|�[�l���g����j�B
        /// </summary>
        public virtual void SetDialogClickedFlag()
        {
            // �J��Ԃ��̃N���b�N��Z���Ԗ������邱�ƂŁA����ăL�����N�^�[�̑䎌���N���b�N���Ă��܂����Ƃ�h���܂��B
            if (ignoreClickTimer > 0f)
            {
                return;
            }
            ignoreClickTimer = nextClickDelay;

            // �N���b�N�I���_�C�A���O���[�h�ł̂ݓK�p
            if (clickMode == ClickMode.ClickOnDialog)
            {
                dialogClickedFlag = true;
            }
        }

        /// <summary>
        /// �{�^�����N���b�N���ꂽ�t���O��ݒ肵�܂��B
        /// </summary>
        public virtual void SetButtonClickedFlag()
        {
            // �N���b�N�������ɂȂ��Ă��Ȃ��ꍇ�̂ݓK�p
            if (clickMode != ClickMode.Disabled)
            {
                SetNextLineFlag();
            }
        }

        #endregion
    }
}