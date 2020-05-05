using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(TMP_Text))]
public class FadeToDeath : MonoBehaviour
{
    [SerializeField]
    CanvasGroup m_canvas = null;
    [SerializeField]
    TMP_Text m_text = null;

    [SerializeField]
    float m_delay = 5.0f;

    [SerializeField]
    float m_step = 0.1f;

    float m_delayTime = 0.0f;

    public void Init(string _name)
    {
        m_text.text = "+1 " + _name + " upgrade part";
        m_delayTime = Time.timeSinceLevelLoad + m_delay;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > m_delayTime) {
            m_canvas.alpha -= m_step;
        }

        if(m_canvas.alpha <= 0.0f) {
            Destroy(gameObject);
        }
    }
}
