using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneStinger : MonoBehaviour
{
    public static ZoneStinger Instance;

    //[SerializeField] private RectTransform m_RectTransform;
    [SerializeField] private TextMeshProUGUI m_StingerText;

    private List<string> m_StingerQueue = new List<string>();
    private string m_LastStinger;
    private Vector3 m_OgPos;
    private Vector3 m_OutPos = new Vector3(Screen.width * 0.5f, Screen.height - 50, 0);

    private bool m_QueueRunning;
    private void Awake()
    {
        Instance = this;
        m_OgPos = transform.position;
    }
    public void SubmitStingerToQueue(string text)
    {
        if (m_LastStinger == text)
            return;

        m_StingerQueue.Add(text);
        m_LastStinger = text;
        if (!m_QueueRunning)
        {
            StartCoroutine(RunQueue());
        }
    }
    IEnumerator RunQueue()
    {
        m_QueueRunning = true;
        float timer = 0;
        while (m_StingerQueue.Count > 0)
        {
            m_StingerText.text = m_StingerQueue[0];

            while (timer < 1)
            {
                //transform.position = Vector3.Lerp(m_OgPos, m_OutPos, timer);
                m_StingerText.color = new Color(1, 1, 1, timer);
                timer += Time.deltaTime;
                yield return null;
            }
            //transform.position = m_OutPos;
            m_StingerText.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(1);

            while (timer > 0)
            {
                //transform.position = Vector3.Lerp(m_OgPos, m_OutPos, timer);
                m_StingerText.color = new Color(1, 1, 1, timer);
                timer -= Time.deltaTime;
                yield return null;
            }

            //transform.position = m_OgPos;
            m_StingerText.color = new Color(1, 1, 1, 0);
            m_StingerQueue.RemoveAt(0);
        }
        m_QueueRunning = false;
    }
}