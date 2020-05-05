using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Défi 2 - Guillaume Dubé
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.Inventory {
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject, IEquatable<Item> {

        [Space]
        [SerializeField]
        string m_name = "No Name";
        public string Name {
            get { return m_name; }
        }

        [SerializeField]
        Sprite m_icon = null;
        public Sprite Icon {
            get { return m_icon; }
        }

        [Space]
        [SerializeField]
        string m_description = "No Description";
        public string Description {
            get { return m_description; }
        }

        public Item(string _name, Sprite _icon, string _description) 
            : this(Guid.NewGuid(), _name, _icon, _description) { 
        }

        public Item(Guid _id, string _name, Sprite _icon, string _description) {
            m_name = _name;
            m_icon = _icon;
            m_description = _description;
        }

        #region Equatable interface.
        public override bool Equals(object obj) {
            return Equals(obj as Item);
        }

        public bool Equals(Item other) {
            return other != null &&
                   base.Equals(other) &&
                   name.Equals(other.name);
        }

        public override int GetHashCode() {
            var hashCode = 2082127350;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            return hashCode;
        }

        public static bool operator ==(Item item1, Item item2) {
            return EqualityComparer<Item>.Default.Equals(item1, item2);
        }

        public static bool operator !=(Item item1, Item item2) {
            return !(item1 == item2);
        }
        #endregion
    }
}
