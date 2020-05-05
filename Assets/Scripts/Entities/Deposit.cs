using UnityEngine;

namespace Thalass.Entities {
    public class Deposit : MonoBehaviour {
        [Header("Loot")]
        [SerializeField]
        ElementType m_element;

        [SerializeField]
        int m_quantity = 0;

        [Header("Modifier")]
        [SerializeField]
        float m_hardness = 100;

        public void Grind(Player.ExtractorController _extractor) {
            m_hardness -= _extractor.Damage;

            if (m_hardness <= 0) {
                _extractor.Harvest(m_element, m_quantity);
                Break();
            }
        }

        //TODO : Break animation.
        public void Break() {
            Destroy(gameObject);
        }
    }

    public enum ElementType {
        Armor,
        Battery,
        Engine,
        Taser,
        Repair
    }
}
