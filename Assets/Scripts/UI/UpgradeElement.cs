using System;
using Thalass.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Thalass.UI {
    public class UpgradeElement : MonoBehaviour, IObserver<Element.Values> {

        [SerializeField]
        Button m_button = null;

        [SerializeField]
        TMP_Text m_count = null;

        [SerializeField]
        Slider m_slider = null;

        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(Element.Values value) {

            string count = value.Count.ToString();
            if (value.Count >= 100)
                count = "99+";

            m_count.text = count + "/" + value.Cost.ToString();
            if (value.Level >= 5)
                m_count.text = "";

            m_slider.value = value.Level;
            m_button.interactable = (value.Level < 5 && value.Count >= value.Cost);
        }
    }
}
