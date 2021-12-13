using UnityEngine;


[CreateAssetMenu(fileName = "Bait", order = 1)]
public class BaitScriptAbleObject : ScriptableObject
{
    public string baitName;



    public int baitLevel;



    private void OnValidate()
    {
        Mathf.Clamp(baitLevel, 0, 3);
    }
}
