using System.Collections.Generic;
using UnityEditor.Profiling;

public static class FrameDataUtilities
{
    public static int FindChildItemByFunctionName(this HierarchyFrameDataView frameData, int parentId, string functionName)
    {
        var childrenIds = new List<int>();
        frameData.GetItemChildren(parentId, childrenIds);
        foreach (var childId in childrenIds)
        {
            var name = frameData.GetItemName(childId);
            if (name == functionName)
                return childId;
        }

        return HierarchyFrameDataView.invalidSampleId;
    }

    public static int FindChildItemByFunctionNameRecursively(this HierarchyFrameDataView frameData, int parentId, string functionName)
    {
        var childrenIds = new List<int>();
        frameData.GetItemChildren(parentId, childrenIds);
        var toVisit = new Queue<int>(childrenIds);
        while (toVisit.Count > 0)
        {
            var id = toVisit.Dequeue();
            var name = frameData.GetItemName(id);
            if (name == functionName)
                return id;

            frameData.GetItemChildren(id, childrenIds);
            foreach (var childId in childrenIds)
                toVisit.Enqueue(childId);
        }

        return HierarchyFrameDataView.invalidSampleId;
    }

    public static int FindChildItemByFunctionSubStrRecursively(this HierarchyFrameDataView frameData, int parentId, string subStr)
    {
        var childrenIds = new List<int>();
        frameData.GetItemChildren(parentId, childrenIds);
        var toVisit = new Queue<int>(childrenIds);
        while (toVisit.Count > 0)
        {
            var id = toVisit.Dequeue();
            var name = frameData.GetItemName(id);
            if (name.Contains(subStr))
                return id;

            frameData.GetItemChildren(id, childrenIds);
            foreach (var childId in childrenIds)
                toVisit.Enqueue(childId);
        }

        return HierarchyFrameDataView.invalidSampleId;
    }
}
