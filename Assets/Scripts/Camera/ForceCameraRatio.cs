using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LetterboxCamera;

namespace LetterboxCamera
{
    [System.Serializable]
    public class CameraRatio
    {
        public enum CameraAnchor
        {
            Center,
            Top,
            Bottom,
            Left,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        public Camera camera;
        public CameraAnchor anchor = CameraAnchor.Center;

        [HideInInspector]
        public Vector2 vectorAnchor;
        private Rect originViewPort;

        public CameraRatio (Camera _camera, Vector2 _anchor) {
            camera = _camera;
            vectorAnchor = _anchor;
            originViewPort = camera.rect;
        }

        public void ResetOriginViewport () {
            originViewPort = camera.rect;
            SetAnchorBasedOnEnum(anchor);
        }

        public void SetAnchorBasedOnEnum (CameraAnchor _anchor) {
            switch (_anchor) {
                case CameraAnchor.Center:
                    vectorAnchor = new Vector2(0.5f, 0.5f);
                    break;
                case CameraAnchor.Top:
                    vectorAnchor = new Vector2(0.5f, 1f);
                    break;
                case CameraAnchor.Bottom:
                    vectorAnchor = new Vector2(0.5f, 0f);
                    break;
                case CameraAnchor.Left:
                    vectorAnchor = new Vector2(0f, 0.5f);
                    break;
                case CameraAnchor.Right:
                    vectorAnchor = new Vector2(1f, 0.5f);
                    break;
                case CameraAnchor.TopLeft:
                    vectorAnchor = new Vector2(0f, 1f);
                    break;
                case CameraAnchor.TopRight:
                    vectorAnchor = new Vector2(1f, 1f);
                    break;
                case CameraAnchor.BottomLeft:
                    vectorAnchor = new Vector2(0f, 0f);
                    break;
                case CameraAnchor.BottomRight:
                    vectorAnchor = new Vector2(1f, 0f);
                    break;
            }
        }

        public void CalculateAndSetCameraRatio (float _width, float _height, bool _horizontalLetterbox) {

            Rect localViewPort = new Rect();

            if (_horizontalLetterbox) { 
                localViewPort.height = _height;
                localViewPort.width = 1;

            } else { 
                localViewPort.height = 1f;
                localViewPort.width = _width;
            }

            Rect screenViewPortHorizontal = new Rect();
            Rect screenViewPortVertical = new Rect();

            screenViewPortHorizontal.width = originViewPort.width;
            screenViewPortHorizontal.height = originViewPort.width * (localViewPort.height / localViewPort.width);
            screenViewPortHorizontal.x = originViewPort.x;
            screenViewPortHorizontal.y = Mathf.Lerp(originViewPort.y, originViewPort.y + (originViewPort.height - screenViewPortHorizontal.height), vectorAnchor.y);

            screenViewPortVertical.width = originViewPort.height * (localViewPort.width / localViewPort.height);
            screenViewPortVertical.height = originViewPort.height;
            screenViewPortVertical.x = Mathf.Lerp(originViewPort.x, originViewPort.x + (originViewPort.width - screenViewPortVertical.width), vectorAnchor.x);
            screenViewPortVertical.y = originViewPort.y;

            if (screenViewPortHorizontal.height >= screenViewPortVertical.height && screenViewPortHorizontal.width >= screenViewPortVertical.width) {
                if (screenViewPortHorizontal.height <= originViewPort.height && screenViewPortHorizontal.width <= originViewPort.width) {
                    camera.rect = screenViewPortHorizontal;
                } else {
                    camera.rect = screenViewPortVertical;
                }
            } else {
                if (screenViewPortVertical.height <= originViewPort.height && screenViewPortVertical.width <= originViewPort.width) {
                    camera.rect = screenViewPortVertical;
                } else {
                    camera.rect = screenViewPortHorizontal;
                }
            }
        }
    }

    [System.Serializable]
    public class ForceCameraRatio : MonoBehaviour
    {
        public Vector2 ratio = new Vector2(16, 9);
        public bool forceRatioOnAwake = true;
        public bool listenForWindowChanges = true;
        public bool createCameraForLetterBoxRendering = true;
        public bool findCamerasAutomatically = true;
        public Color letterBoxCameraColor = new Color(0, 0, 0, 1);

        public List<CameraRatio> cameras;

        public Camera letterBoxCamera;

        private void Start () {
            if (findCamerasAutomatically)
            {
                FindAllCamerasInScene();
            } 
            else if (cameras == null || cameras.Count == 0)
            {
                cameras = new List<CameraRatio>();
            }

            ValidateCameraArray();

            for (int i = 0; i < cameras.Count; i++) 
            {
                cameras[i].ResetOriginViewport();
            }

            if (createCameraForLetterBoxRendering) 
            {
                letterBoxCamera = new GameObject().AddComponent<Camera>();
                letterBoxCamera.backgroundColor = letterBoxCameraColor;
                letterBoxCamera.cullingMask = 0;
                letterBoxCamera.depth = -100;
                letterBoxCamera.farClipPlane = 1;
                letterBoxCamera.useOcclusionCulling = false;
                letterBoxCamera.allowHDR = false;
                letterBoxCamera.clearFlags = CameraClearFlags.Color;
                letterBoxCamera.name = "Letter Box Camera";

                for (int i = 0; i < cameras.Count; i++) 
                {
                    if (cameras[i].camera.depth == -100) 
                    {
                        Debug.LogError(cameras[i].camera.name + " has a depth of -100 and may conflict with the Letter Box Camera in Forced Camera Ratio!");
                    }
                }
            }

            if (forceRatioOnAwake) {
                CalculateAndSetAllCameraRatios();
            }
        }

        private void Update () 
        {
            if (listenForWindowChanges) 
            {
                CalculateAndSetAllCameraRatios();
                if (letterBoxCamera != null) {
                    letterBoxCamera.backgroundColor = letterBoxCameraColor;
                }
            }
        }

        private CameraRatio GetCameraRatioByCamera (Camera _camera) {
            if (cameras == null) {
                return null;
            }

            for (int i = 0; i < cameras.Count; i++) {
                if (cameras[i] != null && cameras[i].camera == _camera) {
                    return cameras[i];
                }
            }

            return null;
        }


        private void ValidateCameraArray() {
            for (int i = cameras.Count - 1; i >= 0; i--) {
                if (cameras[i].camera == null) {
                    cameras.RemoveAt(i);
                }
            }
        }

        public void FindAllCamerasInScene () {
            Camera[] allCameras = FindObjectsOfType<Camera>();
            cameras = new List<CameraRatio>();

            for (int i = 0; i < allCameras.Length; i++) {
                if ((createCameraForLetterBoxRendering || allCameras[i] != letterBoxCamera))
                {
                    cameras.Add(new CameraRatio(allCameras[i], new Vector2(0.5f, 0.5f)));
                }
            }
        }

        public void CalculateAndSetAllCameraRatios () {
            float targetAspect = ratio.x / ratio.y;
            float currentAspect = ((float)Screen.width) / ((float)Screen.height);

            bool horizontalLetterbox = false;
            float fullWidth = targetAspect / currentAspect;
            float fullHeight = currentAspect / targetAspect;

            if (currentAspect > targetAspect) {
                horizontalLetterbox = false;
            }

            for (int i = 0; i < cameras.Count; i++) {
                cameras[i].SetAnchorBasedOnEnum(cameras[i].anchor);
                cameras[i].CalculateAndSetCameraRatio(fullWidth, fullHeight, horizontalLetterbox);
            }
        }

        public void SetCameraAnchor (Camera _camera, Vector2 _anchor) {
            CameraRatio camera = GetCameraRatioByCamera(_camera);
            if (camera != null) {
                camera.vectorAnchor = _anchor;
            }
        }

        public CameraRatio[] GetCameras () {
            if (cameras == null) {
                cameras = new List<CameraRatio>();
            }
            return cameras.ToArray();
        }
    }
}