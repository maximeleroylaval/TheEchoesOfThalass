using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thalass.UI {
    public class UpgradeManager : MonoBehaviour {
        [SerializeField]
        Entities.Submarine m_submarine = null;

        [Space]
        [SerializeField]
        UpgradeElement m_battery = null;
        IDisposable m_batteryObserver = null;

        [SerializeField]
        UpgradeElement m_armor = null;
        IDisposable m_armorObserver = null;

        [SerializeField]
        UpgradeElement m_storage = null;
        IDisposable m_storageObserver = null;

        [SerializeField]
        UpgradeElement m_taser = null;
        IDisposable m_taserObserver = null;

        [SerializeField]
        UpgradeElement m_propulsion = null;
        IDisposable m_propulsionObserver = null;

        void Start() {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            m_batteryObserver = m_submarine.Battery.Subscribe(m_battery);
            m_armorObserver = m_submarine.Armor.Subscribe(m_armor);
            //m_storageObserver = m_submarine.Storage.Subscribe(m_storage);
            m_taserObserver = m_submarine.Weaponry.Subscribe(m_taser);
            m_propulsionObserver = m_submarine.Propulsion.Subscribe(m_propulsion);
        }

        void OnDisable() {
            m_batteryObserver.Dispose();
            m_armorObserver.Dispose();
            //m_storageObserver.Dispose();
            m_taserObserver.Dispose();
            m_propulsionObserver.Dispose();
        }

        public void UpgradeBattery() {
            m_submarine.Battery.Count -= m_submarine.Battery.UpgradeCost;
            m_submarine.Battery.LevelUp();
        }

        public void UpgradeArmor() {
            m_submarine.Armor.Count -= m_submarine.Armor.UpgradeCost;
            m_submarine.Armor.LevelUp();
        }

        public void UpgradeStorage() {
            m_submarine.Battery.Count -= m_submarine.Battery.UpgradeCost;
            m_submarine.Storage.LevelUp();
        }

        public void UpgradeWeaponry() {
            m_submarine.Weaponry.Count -= m_submarine.Weaponry.UpgradeCost;
            m_submarine.Weaponry.LevelUp();
        }

        public void UpgradePropulsion() {
            m_submarine.Propulsion.Count -= m_submarine.Propulsion.UpgradeCost;
            m_submarine.Propulsion.LevelUp();
        }

        public void Submerge() {
            SceneManager.LoadScene("Game");
        }
    }
}
