using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeInCircle.GameLogic
{
    public class CharacterAgent : MonoBehaviour
    {
        public float moveSpeed = 1.0f;

        private Transform trans;

        private void Awake()
        {
            trans = transform;
        }

        public void DoMove(Vector3 moveStep)
        {
            trans.position += moveStep * moveSpeed * Time.deltaTime;
        }

        public void DoRotate(Vector3 forward)
        {
            this.forward = forward;
            MakeYForwardDir(forward);
        }

        private void MakeYForwardDir(Vector3 forward)
        {
            Vector3 currPos = trans.position;
            float dotValue = Vector3.Dot(Vector3.up, forward);
            float angle = Mathf.Acos(dotValue) * Mathf.Rad2Deg;
            Vector3 tempDir = Vector3.Cross(Vector3.up, forward);
            angle *= tempDir.z < 0 ? -1 : 1;
            //trans.RotateAround(currPos, Vector3.forward, angle);
           trans.eulerAngles = new Vector3(0, 0, angle);
            debugAngle = angle;
        }

        private float debugAngle = 0.0f;
        private Vector3 forward;
        private void OnGUI()
        {
            GUILayout.Label("Angle: " + debugAngle);
            GUILayout.Label("Forward: " + forward);
            GUILayout.Label("UP: " + trans.up);
        }

    }
}
