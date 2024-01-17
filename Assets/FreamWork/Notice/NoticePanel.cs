using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class NoticePanel : MonoBehaviour
{
    public NoticeController m_NoticeController;
	enum State
	{
		IN,
		Show,
		Out,
	}

    [Serializable]
	public enum SendColor
	{
		Normal = 0,
		Blue   = 1,
		Red    = 2,
		Green  = 3
	}

	[SerializeField]
	List<Color> m_SendColorSet;

	float m_cTime;

	[SerializeField]
	Image m_Image;

	[SerializeField] 
	float m_InTime;

	[SerializeField] 
	float m_ShowTime;
	
	[SerializeField] 
	float m_OutTime;

	[SerializeField]
	float m_MoveYTime = 0.4f;

	State m_State;

	[SerializeField]
	Text			m_Text;
	[SerializeField]
	GameObject		m_Panel;

	Tween tween;
	Tween tweenY;

	RectTransform trans;

	public void Start(){
		m_State = State.IN;
		trans = GetComponent<RectTransform>();
	}

	public void SetController(NoticeController c) {
		m_NoticeController = c;
	}

	public void MoveY(float Y)
	{
		tweenY.Kill();
		if (trans != null)tweenY = trans.DOAnchorPosY(Y, m_MoveYTime).SetEase(Ease.OutCubic);
	}

	public void SetText(string text) {
		m_Text.text = text;
	}

	public void SetColor(SendColor c) {
		if ((int)c < 0 || (int)c >= m_SendColorSet.Count) {
			Debug.LogError("ƒJƒ‰[Ý’è‚ª“KØ‚Å‚Í‚È‚¢");
		}
		m_Image.color = m_SendColorSet[(int)c];
	}

	public bool Contorl()
	{
		switch (m_State)
		{

			case State.IN:
				tween = trans.DOAnchorPosX(0.0f, m_MoveYTime).SetEase(Ease.OutCubic);

				m_State = State.Show;
				break;

			case State.Show:
				m_cTime += Time.deltaTime;
				if (m_cTime >= m_ShowTime)
				{
					tween = trans.DOAnchorPosX(-200.0f, m_MoveYTime).SetEase(Ease.OutCubic);
					m_State = State.Out;
				}
				break;

			case State.Out:
				if (tween != null && !tween.IsActive() && !tween.IsPlaying())
				{
					return true;
				}
				
				break;
		}
		return false;
	}

}
