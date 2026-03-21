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

    [Header("Random Room Type Settings")]
    public string startNodeID = "A";
    public List<string> bossNodeIDs = new List<string>();

    private Dictionary<string, MapNode> nodeLookup = new Dictionary<string, MapNode>();
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            RandomizeNodeTypes();
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

    void RandomizeNodeTypes()
    {
        foreach (MapNode node in mapNodes)
        {
            if (node.id == startNodeID)
            {
                node.roomType = RoomType.Combat;
            }
            else if (bossNodeIDs.Contains(node.id))
            {
                node.roomType = RoomType.Boss;
            }
            else
            {
                node.roomType = GetRandomRoomType();
            }

            node.sceneName = GetSceneName(node.roomType);
        }
    }

    RoomType GetRandomRoomType()
    {
        int roll = Random.Range(0, 100);

        if (roll < 60)
            return RoomType.Combat;
        else if (roll < 85)
            return RoomType.Shop;
        else
            return RoomType.Upgrade;
    }

    string GetSceneName(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Combat:
                return "CombatScene";

            case RoomType.Shop:
                return "ShopScene";

            case RoomType.Boss:
                return "BossScene";

            case RoomType.Upgrade:
                return "UpgradeRoomScene";

            default:
                return "CombatScene";
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
        unlockedNodes.Clear();
        completedNodes.Clear();
        blockedNodes.Clear();
        currentNodeID = "";

        if (nodeLookup.ContainsKey(startNodeID))
        {
            unlockedNodes.Add(startNodeID);
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

    public void RerollNodeTypes()
    {
        RandomizeNodeTypes();
        BuildLookup();
    }
}
