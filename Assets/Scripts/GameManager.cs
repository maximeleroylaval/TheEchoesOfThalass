using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thalass {
    public class GameManager : MonoBehaviour {

        [SerializeField]
        SubmarineController m_submarine = null;

        [Header("Surfacing")]
        [SerializeField]
        GameObject m_timerObject = null;

        [SerializeField]
        BatteryEventManager m_batteryManager = null;
        System.IDisposable m_batteryObserver = null;

        [SerializeField]
        ArmorEventManager m_armorManager = null;
        System.IDisposable m_armorObserver = null;

        [Space]
        [SerializeField]
        float m_exitHeight = 30.0f;

        [SerializeField]
        float m_exitTimerDelay = 5.0f;
        float m_exitTimerLeft = 5.0f;
        bool m_isExiting = false;

        [Space]
        [SerializeField]
        TMP_Text m_countDown = null;

        bool isInHub = false;
    
        [SerializeField]
        AudioClip softMusic;

        void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SoundManager.instance.PlayMusic(SoundManager.instance.musicSource, SoundManager.instance.ambientUnderWater);
            SoundManager.instance.PlaySingle(SoundManager.instance.musicSource, SoundManager.instance.ambientUnderWater);

            m_batteryObserver = m_submarine.m_submarine.Battery.Subscribe(m_batteryManager);
            m_armorObserver = m_submarine.m_submarine.Armor.Subscribe(m_armorManager);
        }

        void OnDisable()
        {
            m_batteryObserver.Dispose();
            m_armorObserver.Dispose();
        }

        void LateUpdate() {
            if(m_submarine.transform.position.y > m_exitHeight && !isInHub) {
                m_timerObject.SetActive(true);

                if (m_exitTimerLeft < 0 && !isInHub)
                {
                    isInHub = true;
                    m_countDown.text = m_exitTimerLeft.ToString("F3");
                    StartCoroutine(GoToUpgradeSubmarineScene());
                }
                else if (m_isExiting) {
                    m_exitTimerLeft -= Time.fixedDeltaTime;
                    m_countDown.text = m_exitTimerLeft.ToString("F3");
                } else {
                    m_exitTimerLeft = m_exitTimerDelay;
                    m_isExiting = true;
                }
            }
            else {
                m_timerObject.SetActive(false);

                if (m_isExiting)
                    m_isExiting = false;
            }
        }

        private IEnumerator GoToUpgradeSubmarineScene()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Upgrade_Submarine");
        }
    }
}
