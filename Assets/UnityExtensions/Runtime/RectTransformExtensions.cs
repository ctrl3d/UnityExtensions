using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace work.ctrl3d
{
    public static class RectTransformExtensions
    {
        public enum AnchorPreset
        {
            UseDefault,
            FullStretch,
            TopStretch,
            MiddleStretch,
            BottomStretch,
            LeftTop,
            LeftCenter,
            LeftBottom,
            LeftStretch,
            CenterTop,
            Center,
            CenterBottom,
            CenterStretch,
            RightTop,
            RightCenter,
            RightBottom,
            RightStretch,
        }

        [Serializable]
        public struct RectTransformBackup
        {
            public Vector2 anchoredPosition;
            public Vector2 sizeDelta;
            public Vector2 anchorMin;
            public Vector2 anchorMax;
            public Vector2 pivot;
            public Vector3 localScale;

            public RectTransformBackup(RectTransform rectTransform)
            {
                anchoredPosition = rectTransform.anchoredPosition;
                sizeDelta = rectTransform.sizeDelta;
                anchorMin = rectTransform.anchorMin;
                anchorMax = rectTransform.anchorMax;
                pivot = rectTransform.pivot;
                localScale = rectTransform.localScale;
            }
        }

        public static void RestoreFromBackup(this RectTransform rectTransform)
        {
            if (!_backupData.TryGetValue(rectTransform, out var backup)) return;
            rectTransform.anchoredPosition = backup.anchoredPosition;
            rectTransform.sizeDelta = backup.sizeDelta;
            rectTransform.anchorMin = backup.anchorMin;
            rectTransform.anchorMax = backup.anchorMax;
            rectTransform.pivot = backup.pivot;
            rectTransform.localScale = backup.localScale;
        }

        public static bool HasBackup(this RectTransform rectTransform) => _backupData.ContainsKey(rectTransform);
        public static void ClearBackup(this RectTransform rectTransform) => _backupData.Remove(rectTransform); 
        
        public static void BackupState(this RectTransform rectTransform)
        {
            _backupData[rectTransform] = new RectTransformBackup(rectTransform);
        }

        private static Dictionary<RectTransform, RectTransformBackup> _backupData = new();

        public static void SetSize(this RectTransform rectTransform, float width, float height)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        public static void SetSize(this RectTransform rectTransform, Vector2 size)
        {
            rectTransform.sizeDelta = size;
        }

        public static void SetSize(this RectTransform rectTransform, Vector3 size)
        {
            rectTransform.sizeDelta = size;
        }

        public static void SetWidth(this RectTransform rectTransform, float width)
        {
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }

        public static void SetHeight(this RectTransform rectTransform, float height)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        }

        public static void SetPosition(this RectTransform rectTransform, Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }

        public static void SetPosition(this RectTransform rectTransform, Vector3 position)
        {
            rectTransform.anchoredPosition3D = position;
        }

        public static void SetPosition(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchoredPosition = new Vector2(x, y);
        }

        public static void SetPosition(this RectTransform rectTransform, float x, float y, float z)
        {
            rectTransform.anchoredPosition3D = new Vector3(x, y, z);
        }

        public static void SetAnchorPreset(this RectTransform rectTransform, AnchorPreset anchor)
        {
            switch (anchor)
            {
                case AnchorPreset.UseDefault:
                    if (rectTransform.HasBackup()) rectTransform.RestoreFromBackup();
                    else rectTransform.BackupState();

                    break;

                case AnchorPreset.FullStretch:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();

                    SetRectTransform(rectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f),
                        new Vector2(1f, 1f), new Vector2(0.5f, 0.5f));
                    break;

                case AnchorPreset.TopStretch:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), new Vector2(0f, rectTransform.sizeDelta.y),
                        new Vector2(0, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f));
                    break;

                case AnchorPreset.MiddleStretch:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), new Vector2(0f, rectTransform.sizeDelta.y),
                        new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f));
                    break;

                case AnchorPreset.BottomStretch:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), new Vector2(0f, rectTransform.sizeDelta.y),
                        new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f));
                    break;

                case AnchorPreset.LeftTop:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(0f, 1f),
                        new Vector2(0f, 1f), new Vector2(0f, 1f));
                    break;

                case AnchorPreset.LeftCenter:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(0f, 0.5f),
                        new Vector2(0f, 0.5f), new Vector2(0f, 0.5f));
                    break;

                case AnchorPreset.LeftBottom:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(0f, 0f),
                        new Vector2(0f, 0f), new Vector2(0f, 0f));
                    break;

                case AnchorPreset.LeftStretch:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), new Vector2(rectTransform.sizeDelta.x, 0f),
                        new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 0.5f));
                    break;

                case AnchorPreset.CenterTop:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(0.5f, 1f),
                        new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
                    break;

                case AnchorPreset.Center:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta,
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                    break;

                case AnchorPreset.CenterBottom:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(0.5f, 0f),
                        new Vector2(0.5f, 0f), new Vector2(0.5f, 0f));
                    break;

                case AnchorPreset.CenterStretch:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), new Vector2(rectTransform.sizeDelta.x, 0f),
                        new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f));
                    break;

                case AnchorPreset.RightTop:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(1f, 1f),
                        new Vector2(1f, 1f), new Vector2(1f, 1f));
                    break;

                case AnchorPreset.RightCenter:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(1f, 0.5f),
                        new Vector2(1f, 0.5f), new Vector2(1f, 0.5f));
                    break;

                case AnchorPreset.RightBottom:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), rectTransform.sizeDelta, new Vector2(1f, 0f),
                        new Vector2(1f, 0f), new Vector2(1f, 0f));
                    break;

                case AnchorPreset.RightStretch:
                    if (!rectTransform.HasBackup()) rectTransform.BackupState();
                    SetRectTransform(rectTransform, new Vector2(0f, 0f), new Vector2(rectTransform.sizeDelta.x, 0f),
                        new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(1f, 0.5f));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(anchor), anchor, null);
            }
        }

        private static void SetRectTransform(RectTransform rectTransform, Vector2 anchoredPosition, Vector2 sizeDelta,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
        {
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = pivot;
        }

        public static void ResetRectTransform(this RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            rectTransform.pivot = pivot;
        }

        public static void SetPivot(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.pivot = new Vector2(x, y);
        }

        public static void SetAnchorMin(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchorMin = new Vector2(x, y);
        }

        public static void SetAnchorMax(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchorMax = new Vector2(x, y);
        }

        public static void SetAnchor(this RectTransform rectTransform, Vector2 min, Vector2 max)
        {
            rectTransform.anchorMin = min;
            rectTransform.anchorMax = max;
        }

        public static void SetAnchor(this RectTransform rectTransform, float minX, float minY, float maxX,
            float maxY)
        {
            rectTransform.anchorMin = new Vector2(minX, minY);
            rectTransform.anchorMax = new Vector2(maxX, maxY);
        }

        public static void SetOffsetFromPosition(this RectTransform rectTransform, float offsetX, float offsetY)
        {
            var newPosition = rectTransform.anchoredPosition + new Vector2(offsetX, offsetY);
            rectTransform.anchoredPosition = newPosition;
        }

        public static void SetOffsetFromPosition(this RectTransform rectTransform, Vector2 offset)
        {
            rectTransform.anchoredPosition += offset;
        }

        public static void SetScale(this RectTransform rectTransform, float scaleX, float scaleY, float scaleZ = 1f)
        {
            rectTransform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }

        public static void SetRotation(this RectTransform rectTransform, float angle)
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public static void SetPositionRelativeToParent(this RectTransform rectTransform, Vector2 relativePosition)
        {
            if (rectTransform.parent is not RectTransform parentRectTransform) return;
            var parentSize = parentRectTransform.rect.size;
            rectTransform.anchoredPosition =
                new Vector2(parentSize.x * relativePosition.x, parentSize.y * relativePosition.y);
        }

        public static void MoveToFront(this RectTransform rectTransform)
        {
            rectTransform.SetSiblingIndex(rectTransform.parent.childCount - 1);
        }

        public static void MoveToBack(this RectTransform rectTransform)
        {
            rectTransform.SetSiblingIndex(0);
        }

        public static void FitToContent(this RectTransform rectTransform)
        {
            var width = 0f;
            var height = 0f;

            foreach (RectTransform child in rectTransform)
            {
                if (!child.gameObject.activeSelf) continue;
                width = Mathf.Max(width, child.anchoredPosition.x + child.sizeDelta.x);
                height = Mathf.Max(height, child.anchoredPosition.y + child.sizeDelta.y);
            }

            rectTransform.sizeDelta = new Vector2(width, height);
        }

        public static void AlignToTop(this RectTransform rectTransform, float offset = 0)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.anchoredPosition = new Vector2(0, -offset);
        }

        public static void AlignToBottom(this RectTransform rectTransform, float offset = 0)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.pivot = new Vector2(0.5f, 0);
            rectTransform.anchoredPosition = new Vector2(0, offset);
        }

        public static void AlignToLeft(this RectTransform rectTransform, float offset = 0)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.anchoredPosition = new Vector2(offset, 0);
        }

        public static void AlignToRight(this RectTransform rectTransform, float offset = 0)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.anchoredPosition = new Vector2(-offset, 0);
        }

        public static void RotateAroundPivot(this RectTransform rectTransform, float angle)
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public static void SetAspectRatio(this RectTransform rectTransform, float aspectRatio)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x / aspectRatio);
        }

        public static void SetFlexibleSize(this RectTransform rectTransform, float flexibleWidth, float flexibleHeight)
        {
            var layoutElement = rectTransform.GetComponent<LayoutElement>();
            if (layoutElement == null) return;
            layoutElement.flexibleWidth = flexibleWidth;
            layoutElement.flexibleHeight = flexibleHeight;
        }

        public static void CopyFrom(this RectTransform rectTransform, RectTransform source)
        {
            rectTransform.anchorMin = source.anchorMin;
            rectTransform.anchorMax = source.anchorMax;
            rectTransform.anchoredPosition = source.anchoredPosition;
            rectTransform.sizeDelta = source.sizeDelta;
            rectTransform.pivot = source.pivot;
            rectTransform.localScale = source.localScale;
        }
    }
}