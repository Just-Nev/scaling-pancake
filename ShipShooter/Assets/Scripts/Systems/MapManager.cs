using System.Collections;
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

    [Header("Scene Settings")]
    public float sceneLoadDelay = 0.5f;

    private Dictionary<string, MapNode> nodeLookup = new Dictionary<string, MapNode>();
    private bool isTransitioning = false;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isTransitioning = false;
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
            unlockedNodes.Add("A");
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

    List<string> GetParentNodeIDs(string childNodeID)
    {
        List<string> parents = new List<string>();

        foreach (MapNode node in mapNodes)
        {
            if (node.nextNodeIDs.Contains(childNodeID))
            {
                parents.Add(node.id);
            }
        }

        return parents;
    }

    bool AreAllParentsBlocked(string nodeID)
    {
        List<string> parentIDs = GetParentNodeIDs(nodeID);

        if (parentIDs.Count == 0)
            return false;

        foreach (string parentID in parentIDs)
        {
            if (!blockedNodes.Contains(parentID))
            {
                return false;
            }
        }

        return true;
    }

    void BlockBranch(string nodeID)
    {
        if (blockedNodes.Contains(nodeID))
            return;

        blockedNodes.Add(nodeID);
        unlockedNodes.Remove(nodeID);

        MapNode node = GetNode(nodeID);
        if (node == null)
            return;

        foreach (string nextID in node.nextNodeIDs)
        {
            if (AreAllParentsBlocked(nextID))
            {
                BlockBranch(nextID);
            }
        }
    }

    public void SelectNode(string nodeID)
    {
        if (isTransitioning)
            return;

        if (!IsNodeUnlocked(nodeID) || IsNodeCompleted(nodeID))
            return;

        MapNode selectedNode = GetNode(nodeID);
        if (selectedNode == null)
            return;

        // Block only the unchosen branches.
        // Merged nodes stay unblocked if they still have another valid parent.
        if (!string.IsNullOrEmpty(currentNodeID))
        {
            MapNode currentNode = GetNode(currentNodeID);

            if (currentNode != null && currentNode.nextNodeIDs.Count > 1)
            {
                foreach (string siblingID in currentNode.nextNodeIDs)
                {
                    if (siblingID != nodeID)
                    {
                        BlockBranch(siblingID);
                    }
                }
            }
        }

        currentNodeID = nodeID;
        isTransitioning = true;

        StartCoroutine(LoadSceneWithDelay(selectedNode.sceneName));
    }

    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(sceneName);
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
