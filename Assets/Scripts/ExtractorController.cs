using UnityEngine;

/// <summary>
/// Défi 2 - Guillaume Dubé
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.Player {
    public class ExtractorController : MonoBehaviour {

        [SerializeField]
        Entities.Submarine m_submarine = null;

        [SerializeField]
        Inventory.Storage m_storage = null;

        [Space]
        [SerializeField]
        Camera m_camera = null;

        [Header("Effects")]
        [SerializeField]
        ProgressControlV3D m_effect = null;

        [Header("Modifiers")]
        float m_baseDamage = 10.0f;

        float Range {
            get { return m_submarine.Weaponry.Maximum; }
        }

        public float Damage {
            get { return m_submarine.Weaponry.Current; }
        }

        [Space]
        [SerializeField]
        LootMessagePanel m_lootPanel = null;

        void Start() {
            m_storage.SetMaxStacks(Mathf.CeilToInt(m_submarine.Storage.Maximum));
            m_storage.Clear(); //DEBUG.
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetButton("Fire1")) {
                Shoot();
            } else {
                m_effect.extract = false;
                m_effect.triggerExtract = false;
            }
        }

        void Shoot() {
            bool collision = false;
            if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) {
                transform.LookAt(hit.point);
                float distance = Vector3.Distance(transform.position, hit.point);

                //Update weaponry damage.
                m_submarine.Weaponry.Current = m_baseDamage * (1 - distance / Range);

                //If deposit.
                Entities.Deposit deposit = hit.collider.gameObject.GetComponent<Entities.Deposit>();

                if (deposit && distance < Range) {
                    deposit.Grind(this);
                    m_effect.extract = true;
                    m_effect.triggerExtract = true;
                    collision = true;
                }

                //If wildlife.
                WildlifeController wildlife = hit.collider.gameObject.GetComponent<WildlifeController>();

                if (wildlife && distance < Range) {
                    m_effect.extract = true;
                    m_effect.triggerExtract = true;
                    collision = true;
                }

                //If Enemy
                if (hit.collider.gameObject != null && hit.collider.gameObject.GetComponent<EnemyController>() != null)
                {
                    hit.collider.gameObject.GetComponent<EntityController>().takeDamage((int)m_submarine.Weaponry.Current);
                    collision = true;
                }

                if (!collision)
                {
                    m_effect.extract = false;
                    m_effect.triggerExtract = false;
                }
            }
            else
            {
                m_effect.extract = false;
                m_effect.triggerExtract = false;
            }
        }

        public void Harvest(Entities.ElementType _element, int _count) {
            m_lootPanel.Message(System.Enum.GetName(typeof(Entities.ElementType), _element));
            
            switch (_element) {
                case Entities.ElementType.Armor:
                    m_submarine.Armor.Count++;
                    break;
                case Entities.ElementType.Battery:
                    m_submarine.Battery.Count++;
                    break;
                case Entities.ElementType.Engine:
                    m_submarine.Propulsion.Count++;
                    break;
                case Entities.ElementType.Taser:
                    m_submarine.Weaponry.Count++;
                    break;
                case Entities.ElementType.Repair:
                    m_submarine.Count++;
                    break;
            }

        }
    }
}
