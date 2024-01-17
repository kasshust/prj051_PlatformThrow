using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoticeController : SingletonMonoBehaviourFast<NoticeController>
{

    Queue<NoticePanel> m_NoticeQueue;
    public GameObject  m_NoticePanel;

	[SerializeField] GameObject m_Parent;
	[SerializeField] private FMODUnity.EventReference m_NoticeSound;

	private void Start()
    {
		SceneManager.sceneLoaded += SceneLoaded;
		m_NoticeQueue = new Queue<NoticePanel>();
	}

	override protected void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		int num = 0;
		foreach(NoticePanel n in m_NoticeQueue)
		{
			if (n.Contorl()) {
				num++;
			};
		}

        for (int i = 0; i < num; i++)
        {
			Remove();
        }
	}

	public void SendNotice(string text, NoticePanel.SendColor sendColor = NoticePanel.SendColor.Normal)
	{
		Transform p = m_Parent.transform;
		GameObject i = Instantiate(m_NoticePanel, p);
		NoticePanel np = i.GetComponent<NoticePanel>();

		RectTransform t = i.GetComponent<RectTransform>();
		t.anchoredPosition = new Vector2(- 200.0f, t.anchoredPosition.y + m_NoticeQueue.Count * SortY);

		np.SetText(text);
		np.SetColor(sendColor);
		np.SetController(this);
		m_NoticeQueue.Enqueue(np);
		SortBar();

		FMODUnity.RuntimeManager.PlayOneShot(m_NoticeSound, transform.position);

	}

	private void AllRemove() {
		int c = m_NoticeQueue.Count;
		for (int i = 0; i < c; i++)
		{
			Remove();
		}
	}

	public void Remove()
	{
		NoticePanel o = m_NoticeQueue.Dequeue();
		Destroy(o.gameObject);
		SortBar();
	}

	public float SortY = -20.0f;
	private void SortBar()
	{
		int i = 0;
	    foreach(NoticePanel n in m_NoticeQueue)
		{
			n.MoveY( i * SortY );
			i++;
		}
	}

	void SceneLoaded(Scene nextScene, LoadSceneMode mode)
	{
		AllRemove();
	}
}
