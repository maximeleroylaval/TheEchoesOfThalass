using System;
using System.Collections.Generic;

public class Unsubscriber<T> : IDisposable {

    List<IObserver<T>> m_observers;
    IObserver<T> m_observer;

    public Unsubscriber(List<IObserver<T>> _observers, IObserver<T> _observer) {
        m_observers = _observers;
        m_observer = _observer;
    }

    public void Dispose() {
        if (m_observer != null)
            m_observers.Remove(m_observer);
    }
}
