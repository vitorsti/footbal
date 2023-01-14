using UnityEngine;

[CreateAssetMenu(fileName = "SlotHandler", menuName = "ScriptableObject/SlotHandler")]
public class SlotHandler : ScriptableObject
{
    //Essa váriavel irá se adaptar de acordo com o Objeto alocado, tenha isso em mente!
    public Object slot;
}
