using UnityEngine;


[CreateAssetMenu(fileName = "Bait", order = 1, menuName = "Bait")]
public class BaitScriptAbleObject : ScriptableObject
{
    public string baitName;

    [TextArea(2 , 10)]public string baitDescription;

    public int baitLevel;

    private void OnValidate()
    {
        Mathf.Clamp(baitLevel, 0, 3);
    }
}
