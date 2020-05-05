using System;
using UnityEngine;

/// <summary>
/// Défi 6 - Nicolas Mori
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.Entities {

    [CreateAssetMenu(fileName = "Wildlife", menuName = "Entity/Wildlife")]
    public class Wildlife : ScriptableObject {

        [SerializeField]
        [Tooltip("Unique ID | For information only.")]
        string m_id = "";
        public Guid ID { get; private set; } = Guid.NewGuid();

        [Space]
        [SerializeField]
        string m_name = "No Name";
        public string Name {
            get { return m_name; }
        }

        [SerializeField]
        float m_health = 50.0f;
        public float Health {
            get { return m_health; }
        }

        [SerializeField]
        float m_damage = 1.0f;
        public float Damage {
            get { return m_damage; }
        }

        [Space]
        [SerializeField]
        Comportement m_comportement = Comportement.Passive;
        public Comportement Comportement {
            get { return m_comportement; }
        }

        [Header("Modifiers")]
        [SerializeField]
        [Tooltip("Movement speed while roaming.")]
        float m_crusingSpeed = 1.0f;
        public float CrusingSpeed {
            get { return m_crusingSpeed; }
        }

        [Space]
        [SerializeField]
        [Tooltip("Movement speed while reacting.")]
        float m_reactionSpeed = 5.0f;
        public float ReactionSpeed {
            get { return m_reactionSpeed; }
        }

        [SerializeField]
        [Tooltip("Range within which react triggers.")]
        float m_reactionRange = 2.0f;
        public float ReactionRange {
            get { return m_reactionRange; }
        }

        [Space]
        [SerializeField]
        [Tooltip("Range wildlife will return home.")]
        float m_territoryRange = 20.0f;
        public float TerritoryRange {
            get { return m_territoryRange; }
        }

        [SerializeField]
        [Tooltip("Time in seconds before passive calmdown or aggressive attack again.")]
        float m_reactionCooldown = 2.0f;
        public float ReactionCooldown {
            get { return m_reactionCooldown; }
        }

        void OnValidate() {
            m_id = ID.ToString();
        }
    }

    public enum Comportement {
        Passive = 0,
        Aggressive,
        Other
    }
}
