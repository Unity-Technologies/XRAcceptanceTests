using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class GetNodeStates : MonoBehaviour {

    public Material nodeMaterial;

    Dictionary<ulong, GameObject> m_NodeStates;

    void Awake()
    {
        m_NodeStates = new Dictionary<ulong, GameObject>();
    }

    void Update()
    {
        NodesUpdate();
    }

    void NodesUpdate()
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        GameObject tempGameObject;
        Vector3 tempVector3 = Vector3.zero;
        Quaternion tempQuaternion = Quaternion.identity;
        foreach (XRNodeState nodeState in nodeStates)
        {
            if (m_NodeStates.ContainsKey(nodeState.uniqueID))
            {
                m_NodeStates.TryGetValue(nodeState.uniqueID, out tempGameObject);
            }
            else
            {
                AddNewNodeVisual(nodeState);
            }

            m_NodeStates.TryGetValue(nodeState.uniqueID, out tempGameObject);
            if (nodeState.TryGetPosition(out tempVector3))
            {
                tempGameObject.transform.position = (transform.position) + (tempVector3 * transform.lossyScale.x);
            }

            if (nodeState.TryGetRotation(out tempQuaternion))
            {
                tempGameObject.transform.rotation = tempQuaternion;
            }
        }

        bool foundMatch = false;
        foreach (KeyValuePair<ulong, GameObject> nodeState in m_NodeStates)
        {
            foundMatch = false;
            foreach (XRNodeState ns in nodeStates)
            {
                if (ns.uniqueID == nodeState.Key)
                {
                    foundMatch = true;
                    break;
                }
            }
            if (!foundMatch)
            {
                m_NodeStates.TryGetValue(nodeState.Key, out tempGameObject);
                Destroy(tempGameObject);
            }
        }

    }

    void AddNewNodeVisual (XRNodeState nodeState)
    {
        GameObject newNodeVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newNodeVisual.GetComponent<MeshRenderer>().material = nodeMaterial;
        newNodeVisual.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        GameObject TextGameObject = new GameObject();
        TextGameObject.transform.SetParent(newNodeVisual.transform);

        TextMesh newTextMesh = TextGameObject.AddComponent<TextMesh>();
        newTextMesh.text = nodeState.nodeType.ToString();
        newTextMesh.color = Color.white;
        newTextMesh.characterSize = 0.05f;
        newTextMesh.fontSize = 50;

        switch (nodeState.nodeType)
        {
            case XRNode.CenterEye:
                newTextMesh.anchor = TextAnchor.LowerCenter;
                break;
            case XRNode.LeftEye:
                newTextMesh.anchor = TextAnchor.UpperRight;
                break;
            case XRNode.RightEye:
                newTextMesh.anchor = TextAnchor.UpperLeft;
                break;
            case XRNode.Head:
                newTextMesh.anchor = TextAnchor.LowerCenter;
                newTextMesh.text += "\n";
                break;
            case XRNode.LeftHand:
                newTextMesh.anchor = TextAnchor.UpperRight;
                break;
            case XRNode.RightHand:
                newTextMesh.anchor = TextAnchor.UpperLeft;
                break;
        }

        m_NodeStates.Add(nodeState.uniqueID, newNodeVisual);
    }
}
