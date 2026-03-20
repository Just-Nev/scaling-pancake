using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Map Data")]
    public List<MapNode> mapNodes = new List<MapNode>();

    [Header("Run State")]
    public string currentNodeID = "";
    public List<string> unlockedNodes = new List<string>();
    public List<string> completedNodes = new List<string>();
    public List<string> blockedNodes = new List<string>();

    private Dictionary<string, MapNode> nodeLookup = new Dictionary<string, MapNode>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildLookup();
            SetupRun();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void BuildLookup()
    {
        nodeLookup.Clear();

        foreach (MapNode node in mapNodes)
        {
            if (!nodeLookup.ContainsKey(node.id))
            {
                nodeLookup.Add(node.id, node);
            }
        }
    }

    void SetupRun()
    {
        if (unlockedNodes.Count == 0)
        {
            unlockedNodes.Add("A"); // starting node
        }
    }

    public MapNode GetNode(string nodeID)
    {
        if (nodeLookup.ContainsKey(nodeID))
            return nodeLookup[nodeID];

        return null;
    }

    public bool IsNodeUnlocked(string nodeID)
    {
        return unlockedNodes.Contains(nodeID) && !blockedNodes.Contains(nodeID);
    }

    public bool IsNodeCompleted(string nodeID)
    {
        return completedNodes.Contains(nodeID);
    }

    public bool IsNodeBlocked(string nodeID)
    {
        return blockedNodes.Contains(nodeID);
    }

    public void SelectNode(string nodeID)
    {
        if (!IsNodeUnlocked(nodeID) || IsNodeCompleted(nodeID))
            return;

        MapNode selectedNode = GetNode(nodeID);
        if (selectedNode == null)
            return;

        //Branch lock logic
        if (!string.IsNullOrEmpty(currentNodeID))
        {
            MapNode currentNode = GetNode(currentNodeID);

            if (currentNode != null && currentNode.nextNodeIDs.Count > 1)
            {
                foreach (string siblingID in currentNode.nextNodeIDs)
                {
                    if (siblingID != nodeID)
                    {
                        unlockedNodes.Remove(siblingID);

                        if (!blockedNodes.Contains(siblingID))
                        {
                            blockedNodes.Add(siblingID);
                        }
                    }
                }
            }
        }

        currentNodeID = nodeID;
        SceneManager.LoadScene(selectedNode.sceneName);
    }

    public void CompleteCurrentNode()
    {
        if (string.IsNullOrEmpty(currentNodeID))
            return;

        if (!completedNodes.Contains(currentNodeID))
        {
            completedNodes.Add(currentNodeID);
        }

        MapNode currentNode = GetNode(currentNodeID);
        if (currentNode == null)
            return;

        foreach (string nextID in currentNode.nextNodeIDs)
        {
            if (!unlockedNodes.Contains(nextID) && !blockedNodes.Contains(nextID))
            {
                unlockedNodes.Add(nextID);
            }
        }
    }
}
