using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootMessagePanel : MonoBehaviour
{
    [SerializeField]
    FadeToDeath m_message;

    public void Message(string _name) {
        FadeToDeath instance = Instantiate(m_message, gameObject.transform);
        instance.transform.SetAsLastSibling();
        instance.Init(_name);
    }
}
