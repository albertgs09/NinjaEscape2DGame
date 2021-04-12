using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestScriptableObject", menuName = "ScriptableObject/TestScriptableObject")]
public class TestScriptableObject : ScriptableObject
{
    public string myString;
    public int myInt;
    public Sprite[] spriteArray;
    
}
