using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VarCo
{
    public static class Mouse3D
    {
        private static Camera mainCamera;

        /// <summary>
        /// Returns true if mouse pointing on UI object
        /// </summary>
        public static bool IsOverUI()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        /// <summary>
        /// Cached main camera reference
        /// </summary>
        public static Camera MainCamera
        {
            get
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }

		/// <summary>
		/// Returns a point in worldspace relative to mouse position and distance from camera
		/// </summary>
		/// <param name="distanceFromCamera"></param>
		/// <returns></returns>
		public static Vector3 WorldPoint(float distanceFromCamera = 0)
        {
            Vector3 mousePosition = Vector3.zero;
            mousePosition.x = Input.mousePosition.x;
            mousePosition.y = Input.mousePosition.y;
            mousePosition.z = distanceFromCamera;
            return MainCamera.ScreenToWorldPoint(mousePosition);
        }

        /// <summary>
        /// Returns a ray from camera relative to mouse position
        /// </summary>
        /// <returns></returns>
        public static Ray Ray()
        {
            return MainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
