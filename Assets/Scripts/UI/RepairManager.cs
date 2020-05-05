using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thalass.UI {
    public class RepairManager : MonoBehaviour {

        [SerializeField]
        Entities.Submarine m_submarine = null;

        [Space]
        [SerializeField]
        EnergyElement m_battery = null;
        IDisposable m_batteryObserver = null;
        [SerializeField]
        float m_batteryRepairRate = 5.0f;

        [SerializeField]
        RepairElement m_armor = null;
        IDisposable m_armorObserver = null;
        [SerializeField]
        float m_armorRepairRate = 5.0f;

        void Start() {
            m_batteryObserver = m_submarine.Battery.Subscribe(m_battery);
            m_armorObserver = m_submarine.Armor.Subscribe(m_armor);
        }

        void Update() {
            RepairBattery();
        }

        void OnDisable() {
            m_batteryObserver.Dispose();
            m_armorObserver.Dispose();
        }

        public void RepairBattery() {
            m_submarine.Battery.Current += m_batteryRepairRate * Time.deltaTime;
        }

        public void RepairArmor() {
            m_submarine.Count--;
            m_submarine.Armor.Current += m_armorRepairRate;
        }
    }
}
