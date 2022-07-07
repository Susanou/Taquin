using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object to share the timer in between scenes
/// since we are using this as the score for the player
/// Also used to remember the best score of the player
/// </summary>
[CreateAssetMenu(fileName = "New Float", menuName = "Values/FloatValue", order = 1)]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{

    public float initialValue; //Value of the object (Stays the same throught the game)

    [HideInInspector] 
    public float RuntimeValue; //Value used during the game

    public void OnAfterDeserialize()
    {
        RuntimeValue = initialValue; //Reset value once the game stops
    }

    public void OnBeforeSerialize()
    {}

}