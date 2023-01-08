using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Game Object Instances/Blocks/Block")]
public class Block : ScriptableObject
{
    [SerializeField] private string uniqueName;
    [SerializeField] private int uniqueID;
    
    public string UniqueName { get => uniqueName; }
    public int UniqueID { get => uniqueID; }
    
    public Block(int uniqueID)
    {
        this.uniqueID = uniqueID;
    }
}