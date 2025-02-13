using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = string.Format("V {0}",Application.version);
    }
}
