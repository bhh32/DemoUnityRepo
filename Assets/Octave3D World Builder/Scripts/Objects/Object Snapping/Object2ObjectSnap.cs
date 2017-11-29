#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace O3DWB
{
    public static class Object2ObjectSnap
    {
        public struct SnapResult
        {
            private bool _wasSnapped;
            private Vector3 _snapPivot;
            private Vector3 _snapDestination;

            public bool WasSnapped { get { return _wasSnapped; } }
            public Vector3 SnapPivot { get { return _snapPivot; } }
            public Vector3 SnapDestination { get { return _snapDestination; } }

            public SnapResult(bool wasSnapped, Vector3 snapPivot, Vector3 snapDestination)
            {
                _wasSnapped = wasSnapped;
                _snapPivot = snapPivot;
                _snapDestination = snapDestination;
            }

            public static SnapResult GetWasNotSnapped()
            {
                return new SnapResult(false, Vector3.zero, Vector3.zero);
            }
        }

        public static SnapResult Snap(List<GameObject> roots, float snapEpsilon, List<GameObject> ignoreDestSnapObjects)
        {
            if (ignoreDestSnapObjects == null) ignoreDestSnapObjects = new List<GameObject>();

            Vector3 snapDestPt = Vector3.zero, snapPivotPt = Vector3.zero;
            float minSnapDistance = float.MaxValue;

            foreach(var hierarchyRoot in roots)
            {
                List<GameObject> meshObjectsInHierarchy = hierarchyRoot.GetHierarchyObjectsWithMesh();
                List<GameObject> spriteObjectsInHierarchy = hierarchyRoot.GetHierarchyObjectsWithSprites();
                if (meshObjectsInHierarchy.Count == 0 && spriteObjectsInHierarchy.Count == 0) return SnapResult.GetWasNotSnapped();

                Box hierarchyWorldAABB = hierarchyRoot.GetHierarchyWorldBox();
                if (!hierarchyWorldAABB.IsValid()) return SnapResult.GetWasNotSnapped();
                Box hierarchyQueryBox = hierarchyWorldAABB;
                hierarchyQueryBox.Size = hierarchyQueryBox.Size + Vector3.one * snapEpsilon;

                List<GameObject> nearbyObjects = Octave3DScene.Get().OverlapBox(hierarchyQueryBox);
                if (nearbyObjects.Count == 0) return SnapResult.GetWasNotSnapped();
                nearbyObjects.RemoveAll(item => item.transform.IsChildOf(hierarchyRoot.transform) ||
                                        ignoreDestSnapObjects.Contains(item));

                var object2ObjectSnapDatabase = Object2ObjectBoxSnapDatabase.Instance;
                List<GameObject> sourceObjects = hierarchyRoot.GetAllChildrenIncludingSelf();
                if (sourceObjects.Count > 1 || spriteObjectsInHierarchy.Count == 0)
                {
                    foreach (var sourceObject in sourceObjects)
                    {
                        Object2ObjectBoxSnapData sourceSnapData = object2ObjectSnapDatabase.GetObject2ObjectBoxSnapData(sourceObject);
                        if (sourceSnapData == null) continue;

                        var sourceSnapBoxes = sourceSnapData.GetWorldSnapBoxes();
                        foreach (var destObject in nearbyObjects)
                        {
                            Object2ObjectBoxSnapData destSnapData = object2ObjectSnapDatabase.GetObject2ObjectBoxSnapData(destObject);
                            if (destSnapData == null) continue;

                            var destSnapBoxes = destSnapData.GetWorldSnapBoxes();

                            foreach (var sourceSnapBox in sourceSnapBoxes)
                            {
                                var sourceBoxPoints = sourceSnapBox.GetCenterAndCornerPoints();
                                foreach (var destSnapBox in destSnapBoxes)
                                {
                                    var destBoxPoints = destSnapBox.GetCenterAndCornerPoints();
                                    foreach (var srcPt in sourceBoxPoints)
                                    {
                                        foreach (var destPt in destBoxPoints)
                                        {
                                            float distance = (destPt - srcPt).magnitude;
                                            if (distance < minSnapDistance)
                                            {
                                                minSnapDistance = distance;
                                                snapDestPt = destPt;
                                                snapPivotPt = srcPt;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<Vector3> hierarchyBoxCornerPoints = hierarchyWorldAABB.GetCornerPoints();
                    foreach (GameObject gameObject in nearbyObjects)
                    {
                        Box objectWorldBox = Box.GetInvalid();

                        Mesh objectMesh = gameObject.GetMeshFromFilterOrSkinnedMeshRenderer();
                        if (objectMesh != null) objectWorldBox = gameObject.GetMeshWorldBox();
                        if (objectWorldBox.IsInvalid() && gameObject.HasSpriteRendererWithSprite()) objectWorldBox = gameObject.GetNonMeshWorldBox();

                        if (objectWorldBox.IsInvalid()) continue;

                        List<Vector3> worldBoxCornerPoints = objectWorldBox.GetCornerPoints();
                        foreach (Vector3 hierarchyBoxPt in hierarchyBoxCornerPoints)
                        {
                            foreach (Vector3 objectMeshBoxPt in worldBoxCornerPoints)
                            {
                                float distance = (hierarchyBoxPt - objectMeshBoxPt).magnitude;
                                if (distance < minSnapDistance)
                                {
                                    minSnapDistance = distance;
                                    snapDestPt = objectMeshBoxPt;
                                    snapPivotPt = hierarchyBoxPt;
                                }
                            }
                        }
                    }
                }
            }

            if (minSnapDistance < snapEpsilon)
            {
                foreach(var root in roots) ObjectHierarchySnap.Snap(root, snapPivotPt, snapDestPt);
                return new SnapResult(true, snapPivotPt, snapDestPt);
            }

            return SnapResult.GetWasNotSnapped();
        }

        public static SnapResult Snap(GameObject root, float snapEpsilon, List<GameObject> ignoreDestSnapObjects)
        {
            if (ignoreDestSnapObjects == null) ignoreDestSnapObjects = new List<GameObject>();

            Vector3 snapDestPt = Vector3.zero, snapPivotPt = Vector3.zero;
            float minSnapDistance = float.MaxValue;

            List<GameObject> meshObjectsInHierarchy = root.GetHierarchyObjectsWithMesh();
            List<GameObject> spriteObjectsInHierarchy = root.GetHierarchyObjectsWithSprites();
            if (meshObjectsInHierarchy.Count == 0 && spriteObjectsInHierarchy.Count == 0) return SnapResult.GetWasNotSnapped();

            Box hierarchyWorldAABB = root.GetHierarchyWorldBox();
            if (!hierarchyWorldAABB.IsValid()) return SnapResult.GetWasNotSnapped();
            Box hierarchyQueryBox = hierarchyWorldAABB;
            hierarchyQueryBox.Size = hierarchyQueryBox.Size + Vector3.one * snapEpsilon;

            List<GameObject> nearbyObjects = Octave3DScene.Get().OverlapBox(hierarchyQueryBox);
            if (nearbyObjects.Count == 0) return SnapResult.GetWasNotSnapped();
            nearbyObjects.RemoveAll(item => item.transform.IsChildOf(root.transform) ||
                                    ignoreDestSnapObjects.Contains(item));

            var object2ObjectSnapDatabase = Object2ObjectBoxSnapDatabase.Instance;
            List<GameObject> sourceObjects = root.GetAllChildrenIncludingSelf();
            if (sourceObjects.Count > 1 || spriteObjectsInHierarchy.Count == 0)
            {
                foreach (var sourceObject in sourceObjects)
                {
                    Object2ObjectBoxSnapData sourceSnapData = object2ObjectSnapDatabase.GetObject2ObjectBoxSnapData(sourceObject);
                    if (sourceSnapData == null) continue;

                    var sourceSnapBoxes = sourceSnapData.GetWorldSnapBoxes();
                    foreach (var destObject in nearbyObjects)
                    {
                        Object2ObjectBoxSnapData destSnapData = object2ObjectSnapDatabase.GetObject2ObjectBoxSnapData(destObject);
                        if (destSnapData == null) continue;

                        var destSnapBoxes = destSnapData.GetWorldSnapBoxes();

                        foreach (var sourceSnapBox in sourceSnapBoxes)
                        {
                            var sourceBoxPoints = sourceSnapBox.GetCenterAndCornerPoints();
                            foreach (var destSnapBox in destSnapBoxes)
                            {
                                var destBoxPoints = destSnapBox.GetCenterAndCornerPoints();
                                foreach (var srcPt in sourceBoxPoints)
                                {
                                    foreach (var destPt in destBoxPoints)
                                    {
                                        float distance = (destPt - srcPt).magnitude;
                                        if (distance < minSnapDistance)
                                        {
                                            minSnapDistance = distance;
                                            snapDestPt = destPt;
                                            snapPivotPt = srcPt;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                List<Vector3> hierarchyBoxCornerPoints = hierarchyWorldAABB.GetCornerPoints();
                foreach (GameObject gameObject in nearbyObjects)
                {
                    Box objectWorldBox = Box.GetInvalid();

                    Mesh objectMesh = gameObject.GetMeshFromFilterOrSkinnedMeshRenderer();
                    if (objectMesh != null) objectWorldBox = gameObject.GetMeshWorldBox();
                    if (objectWorldBox.IsInvalid() && gameObject.HasSpriteRendererWithSprite()) objectWorldBox = gameObject.GetNonMeshWorldBox();

                    if (objectWorldBox.IsInvalid()) continue;

                    List<Vector3> worldBoxCornerPoints = objectWorldBox.GetCornerPoints();
                    foreach (Vector3 hierarchyBoxPt in hierarchyBoxCornerPoints)
                    {
                        foreach (Vector3 objectMeshBoxPt in worldBoxCornerPoints)
                        {
                            float distance = (hierarchyBoxPt - objectMeshBoxPt).magnitude;
                            if (distance < minSnapDistance)
                            {
                                minSnapDistance = distance;
                                snapDestPt = objectMeshBoxPt;
                                snapPivotPt = hierarchyBoxPt;
                            }
                        }
                    }
                }
            }

            if (minSnapDistance < snapEpsilon) 
            {
                ObjectHierarchySnap.Snap(root, snapPivotPt, snapDestPt);
                return new SnapResult(true, snapPivotPt, snapDestPt);
            }

            return SnapResult.GetWasNotSnapped();
        }
    }
}
#endif