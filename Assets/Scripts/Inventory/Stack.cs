using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Défi 2 - Guillaume Dubé
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.Inventory {
    [Serializable]
    public class Stack : IEquatable<Stack> {

        public const int MAX_PER_STACK = 10;

        [SerializeField]
        Item m_item = null;

        public string Name {
            get { return (m_item) ? m_item.Name : "Empty"; }
        }

        public Sprite Icon {
            get { return (m_item) ? m_item.Icon : null; }
        }

        public string Description {
            get { return (m_item) ? m_item.Description : "No Description"; }
        }

        [SerializeField]
        int m_quantity = 0;
        public int Quantity { get { return m_quantity; } }

        public int Free { get { return MAX_PER_STACK - Quantity; } }

        public Stack(Item _item, int _quantity) {
            m_item = _item;
            m_quantity = _quantity;
        }

        public bool Add(int _value) {
            if ((Quantity + _value) > MAX_PER_STACK)
                return false;

            m_quantity += _value;
            return true;
        }

        public bool Remove(int _value) {
            if ((Quantity - _value) < 0)
                return false;

            m_quantity -= _value;
            return true;
        }

        public void Empty() {
            Remove(Quantity);
        }

        #region Equatable
        public override bool Equals(object obj) {
            return Equals(obj as Stack);
        }

        public bool Equals(Stack other) {
            return other != null &&
                   EqualityComparer<Item>.Default.Equals(m_item, other.m_item) &&
                   Quantity == other.Quantity;
        }

        public override int GetHashCode() {
            return m_item.GetHashCode();
        }

        public static bool operator ==(Stack stack1, Stack stack2) {
            return EqualityComparer<Stack>.Default.Equals(stack1, stack2);
        }

        public static bool operator !=(Stack stack1, Stack stack2) {
            return !(stack1 == stack2);
        }

        public static bool operator <(Stack stack1, Stack stack2) {
            return (stack1.m_item == stack2.m_item && stack1.Quantity < stack2.Quantity);
        }

        public static bool operator >(Stack stack1, Stack stack2) {
            return (stack1.m_item == stack2.m_item && stack1.Quantity > stack2.Quantity);
        }
        #endregion
    }
}
