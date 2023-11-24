using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinCodeDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _joinCodeText;

    private void Start()
    {
        _joinCodeText.text = HostSingleton.Instance.Host.JoinCode;
    }
}
