using UnityEngine;

/// <summary>
/// Défi 1 - Marc Lauzon
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.UI {
    public class CompassController : MonoBehaviour {

        [SerializeField]
        RectTransform m_rect = null;

        [SerializeField]
        SubmarineController m_submarine = null;

        float m_angle = 0;
        public float Angle {
            get {
                return m_angle;
            }

            private set {
                m_angle = value % 360;

                if (m_angle > 180) m_angle -= 360;
                else if (m_angle < -180) m_angle += 360;
            }
        }

        void FixedUpdate() {
            Angle = m_submarine.transform.rotation.eulerAngles.y;
        }

        void LateUpdate() {
            m_rect.anchoredPosition = new Vector2(Angle * -2, 0);
        }
    }
}
