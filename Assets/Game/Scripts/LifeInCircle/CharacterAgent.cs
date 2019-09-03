using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeInCircle.GameLogic
{
    public enum EN_CharacterState
    {
        Stay,
        Moving,
    }

    public class CharacterAgent : MonoBehaviour
    {
        public bool IsLocalPlayer;
        public float moveSpeed = 1.0f;

        private Transform trans;

        private Vector3 rightScale = new Vector3(-1, 1, 1);
        private Vector3 leftScale = new Vector3(1, 1, 1);
        private Vector3 stay = Vector3.zero;

        private EN_CharacterState state;

        private Vector3 debug_moveVector;

        private void Awake()
        {
            trans = transform;
            state = EN_CharacterState.Stay;
        }

        private void Start()
        {
            OnDebugInit();
        }

        public void DoMove(Vector3 normMoveVector)
        {
            debug_moveVector = normMoveVector;
            if(normMoveVector == stay)
            {
                state = EN_CharacterState.Stay;
                return;
            }
            state = EN_CharacterState.Moving;
            UpdateCharacterView(normMoveVector);
            trans.position += normMoveVector * moveSpeed * Time.deltaTime;
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

        private void UpdateCharacterView(Vector3 normMoveVector)
        {
            if(normMoveVector.x > 0)
            {
                trans.localScale = rightScale;
            }
            else if(normMoveVector.x < 0)
            {
                trans.localScale = leftScale;
            }
        }


        private GUIStyle guiStyle;
        private void OnDebugInit()
        {
            guiStyle = new GUIStyle();
            guiStyle.fontSize = 20;
            GUIUtil.Inst.AddCall(this.DrawGUI);
        }

        private void DrawGUI()
        {
            GUILayout.Label(string.Format("State: {0}", state), guiStyle);
            GUILayout.Label(string.Format("Norm Move Vector: {0}", debug_moveVector), guiStyle);
        }

        private float debugAngle = 0.0f;
        private Vector3 forward;
        //private void OnGUI()
        //{
        //    //GUI.color = Color.red;
           
        //}

    }
}
