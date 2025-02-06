using UnityEngine;

[CreateAssetMenu(fileName = "Power_Card", menuName = "Power_Card", order = 2)]
public class PowerCard : ScriptableObject
{
    [SerializeField] public string power_ID;
    [SerializeField] public int powerRank;
    [SerializeField] public string powerName;
    [SerializeField] public Sprite powerImage;
    [SerializeField, TextArea, Space(5)] public string powerDescription;
    //[SerializeField] public Talent powerEffect;
}
