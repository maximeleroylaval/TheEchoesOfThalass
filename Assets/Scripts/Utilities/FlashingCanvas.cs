using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FlashingCanvas : MonoBehaviour
{
    CanvasGroup m_canvasGroup = null;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float m_alphaStep = 0.1f;

    void Start() {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (m_canvasGroup.interactable) {
            if (m_canvasGroup.alpha < 1.0f)
                m_canvasGroup.alpha += m_alphaStep;
            else
                m_canvasGroup.interactable = false;

        } else {
            if (m_canvasGroup.alpha > 0.0f)
                m_canvasGroup.alpha -= m_alphaStep;
            else
                m_canvasGroup.interactable = true;

        }
    }
}
