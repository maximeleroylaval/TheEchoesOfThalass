using System;
using System.Collections.Generic;
using System.Linq;
using Thalass.Entities;
using UnityEngine;

/// <summary>
/// Défi 2 - Guillaume Dubé
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.Inventory {
    [CreateAssetMenu(fileName = "Storage", menuName = "Inventory/Storage")]
    public class Storage : ScriptableObject, IObservable<Storage.Values[]> {

        [SerializeField]
        List<Stack> m_stacks = new List<Stack>();

        public int MaximumStacks { get; private set; } = 5;

        public void SetMaxStacks(int _count) {
            MaximumStacks = _count;
        }

        public Stack Add(Stack _stack) {
            //Find all stacks.
            List<Stack> stacks = m_stacks.FindAll(x => x.Name == _stack.Name);

            //Fill stacks.
            foreach(Stack stack in stacks) {
                if(stack.Free >= _stack.Quantity) {
                    stack.Add(_stack.Quantity);
                    _stack.Empty();
                    break;
                } else {
                    _stack.Remove(stack.Free);
                    stack.Add(stack.Free);
                }
            }

            //Leftovers?
            if(_stack.Quantity > 0) {
                if (m_stacks.Count >= MaximumStacks) {
                    return _stack;
                } else {
                    m_stacks.Add(_stack);
                }
            }
            
            return null;
        }

        public bool Remove(Item _item, int _quantity) {
            //Find all stacks.
            List<Stack> stacks = m_stacks.FindAll(x => x.Name == _item.Name);

            //Count ressources.
            int count = stacks.Sum(x => x.Quantity);

            //Insufficient ressources.
            if (count > _quantity)
                return false;

            //Deplete stacks.
            foreach(Stack stack in stacks) {
                if(stack.Quantity >= _quantity) {
                    _quantity -= Stack.MAX_PER_STACK;
                    m_stacks.Remove(stack);
                } else {
                    stack.Remove(_quantity);
                    _quantity = 0;
                    break;
                }
            }

            return true;
        }

        public int Count(Item _item) {
            //Find all stacks.
            List<Stack> stacks = m_stacks.FindAll(x => x.Name == _item.Name);

            //Count stacks.
            return m_stacks.Sum(x => x.Quantity);
        }

        public void Clear() {
            m_stacks.Clear();
        }

        #region Observable pattern
        public struct Values {
            public string Name { get; }
            public Sprite Icon { get; }
            public string Description { get; }
            public int Quantity { get; }

            public Values(Stack _stack) {
                Name = _stack.Name;
                Icon = _stack.Icon;
                Description = _stack.Description;
                Quantity = _stack.Quantity;
            }
        }

        List<IObserver<Values[]>> m_observers = new List<IObserver<Values[]>>();

        /// <summary>
        /// Register an observer to warn on value change.
        /// </summary>
        /// <param name="_observer">Observing callback.</param>
        /// <returns>Object for unsubscribing.</returns>
        public IDisposable Subscribe(IObserver<Values[]> _observer) {
            if (!m_observers.Contains(_observer))
                m_observers.Add(_observer);

            return new Unsubscriber<Values[]>(m_observers, _observer);
        }

        /// <summary>
        /// Warn every registered observers about a value change.
        /// </summary>
        void Rollout() {
            Values[] values = new Values[MaximumStacks];
            for (int i = 0; i < MaximumStacks; i++)
                values[i] = new Values(m_stacks[i]);

            foreach (IObserver<Values[]> observer in m_observers)
                observer.OnNext(values);
        }
        #endregion
    }
}