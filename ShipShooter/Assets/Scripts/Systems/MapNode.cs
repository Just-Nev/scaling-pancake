using System.Collections.Generic;

[System.Serializable]
public class MapNode
{
    public string id;
    public RoomType roomType;
    public string sceneName;
    public List<string> nextNodeIDs = new List<string>();
}
