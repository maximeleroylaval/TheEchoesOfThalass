using System;
using Thalass.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Thalass.UI {
    public class RepairElement : MonoBehaviour, IObserver<Element.Values> {

        [SerializeField]
        Submarine m_submarine = null;

        [SerializeField]
        TMP_Text m_plate = null;

        [SerializeField]
        Slider m_slider = null;

        [SerializeField]
        Button m_button = null;

        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(Element.Values value) {
            m_slider.maxValue = value.Maximum;
            m_slider.value = value.Current;

            m_plate.text = m_submarine.Count.ToString();

            m_button.interactable = (value.Current < value.Maximum && m_submarine.Count > 0);
        }
    }
}
