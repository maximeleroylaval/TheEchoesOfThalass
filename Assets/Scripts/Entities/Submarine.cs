using UnityEngine;

/// <summary>
/// Défi 1 - Marc Lauzon
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.Entities {

    [CreateAssetMenu(fileName = "Submarine", menuName = "Entity/Submarine")]
    public class Submarine : ScriptableObject {

        [Header("Upgradables")]
        [SerializeField]
        Element m_battery = null;
        public Element Battery {
            get { return m_battery; }
        }

        [SerializeField]
        Element m_armor = null;
        public Element Armor {
            get { return m_armor; }
        }
        
        [SerializeField]
        Element m_propulsion = null;
        public Element Propulsion {
            get { return m_propulsion; }
        }

        [SerializeField]
        Element m_weaponry = null;
        public Element Weaponry {
            get { return m_weaponry; }
        }

        [SerializeField]
        Element m_storage = null;
        public Element Storage {
            get { return m_storage; }
        }

        [SerializeField]
        int m_count = 0;
        public int Count {
            get { return m_count; }
            set { m_count = (value <= 0) ? 0 : value; }
        }
    }
}
