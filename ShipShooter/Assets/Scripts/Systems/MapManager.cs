using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public string currentNode = "";
    public List<string> completedNodes = new List<string>();
    public List<string> unlockedNodes = new List<string>();

    private Dictionary<string, string> nodeToScene = new Dictionary<string, string>();
    private Dictionary<string, List<string>> nodeConnections = new Dictionary<string, List<string>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupMap()
    {
        nodeToScene["A"] = "CombatScene1";
        nodeToScene["B"] = "CombatScene2";
        nodeToScene["C"] = "CombatScene3";
        nodeToScene["D"] = "CombatScene4";
        nodeToScene["E"] = "CombatScene4";

        nodeConnections["A"] = new List<string> { "B", "C" };
        nodeConnections["B"] = new List<string> { "D" };
        nodeConnections["C"] = new List<string> { "E" };
        nodeConnections["D"] = new List<string>();
        nodeConnections["E"] = new List<string>();

        if (unlockedNodes.Count == 0)
        {
            unlockedNodes.Add("A");
        }
    }

    public bool IsNodeUnlocked(string nodeID)
    {
        return unlockedNodes.Contains(nodeID);
    }

    public bool IsNodeCompleted(string nodeID)
    {
        return completedNodes.Contains(nodeID);
    }

    public void SelectNode(string nodeID)
    {
        if (!unlockedNodes.Contains(nodeID))
            return;

        if (currentNode != "" && nodeConnections.ContainsKey(currentNode))
        {
            foreach (string connectedNode in nodeConnections[currentNode])
            {
                if (connectedNode != nodeID)
                {
                    unlockedNodes.Remove(connectedNode);
                }
            }
        }

        currentNode = nodeID;
        SceneManager.LoadScene(nodeToScene[nodeID]);
    }

    public void CompleteCurrentNode()
    {
        if (currentNode == "")
            return;

        if (!completedNodes.Contains(currentNode))
        {
            completedNodes.Add(currentNode);
        }

        if (nodeConnections.ContainsKey(currentNode))
        {
            foreach (string nextNode in nodeConnections[currentNode])
            {
                if (!unlockedNodes.Contains(nextNode))
                {
                    unlockedNodes.Add(nextNode);
                }
            }
        }
    }
}
