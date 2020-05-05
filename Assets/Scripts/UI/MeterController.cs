using System;
using Thalass.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Défi 1 - Marc Lauzon
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass.UI {
    public class MeterController : MonoBehaviour, IObserver<Entities.Element.Values> {
        const float MIN_LIMIT = 0.155f;
        const float MAX_LIMIT = 0.845f;

        float Scale => (MAX_LIMIT - MIN_LIMIT) + MIN_LIMIT;

        [SerializeField]
        Image m_fill = null;
        public float Value {
            set => m_fill.fillAmount = Mathf.Clamp01(value) * Scale;
        }

        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnNext(Element.Values value) {
            Value = value.Current / value.Maximum;
        }
    }
}
