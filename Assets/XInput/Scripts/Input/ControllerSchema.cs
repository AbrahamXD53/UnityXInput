using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInput;

[CreateAssetMenu(fileName = "ControllerSchema", menuName = "Customs/Schema", order = 2)]
public class ControllerSchema : ScriptableObject
{
    public new string name = "Default";
    public ActionButton[] actionButtons;
    public ActionAxis[] actionAxis;
}
