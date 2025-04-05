
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 3)]
public class PlayerData : ScriptableObject
{
    public float maxHealth = 100;
    public float maxStamina = 3.5f;
    public float baseWalkSpeed = 5f;
    public float baseSprintSpeed = 15f;
    public float mouseSensitivity = 7f;
    public float baseJumpForce = 10f;
}
