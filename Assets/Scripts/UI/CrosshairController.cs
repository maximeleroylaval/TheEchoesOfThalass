using UnityEngine;

/// <summary>
/// Défi 1 - Marc Lauzon
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.UI {
    public class CrosshairController : MonoBehaviour {
        RectTransform m_rect;

        void Start() {
            m_rect = GetComponent<RectTransform>();
        }

        void Update() {
            m_rect.position = Input.mousePosition;
        }
    }
}
