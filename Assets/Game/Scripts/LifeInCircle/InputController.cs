using UnityEngine;
using System.Collections;

namespace LifeInCircle.GameLogic
{
    public class InputController : MonoBehaviour
    {
        public CharacterAgent agent;

        private Vector3 moveVector;    // normalize move vector
        private Vector3 forwardVector; // normalize forward vector

        private void Update()
        {
            ProcessAxis();
            //ProcessKeyboardInput();
            agent.DoMove(moveVector);
            //agent.DoRotate(forwardVector);
        }

        private void ProcessKeyboardInput()
        {
            #region Move
            // Left
            if (Input.GetKeyDown(KeyCode.A))
            {
                moveVector.x = -1;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                if(moveVector.x < 0)
                {
                    moveVector.x = 0;
                }
            }

            // Right
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveVector.x = 1;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                if(moveVector.x > 0)
                {
                    moveVector.x = 0;
                }
            }

            // Up
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveVector.y = 1;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                if(moveVector.y > 0)
                {
                    moveVector.y = 0;
                }
            }

            // Down
            if (Input.GetKeyDown(KeyCode.S))
            {
                moveVector.y = -1;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                if(moveVector.y < 0)
                {
                    moveVector.y = 0;
                }
            }
            #endregion

            #region rotate
            //Vector3 screenPos = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            //forwardVector = (mousePos - screenPos).normalized;
            #endregion
        }

        private void ProcessAxis()
        {
            float acceleration = 2.0f;
            float x = Input.GetAxis("Horizontal") * acceleration;
            float y = Input.GetAxis("Vertical") * acceleration;
            x = Mathf.Clamp(x, -1.0f, 1.0f);
            y = Mathf.Clamp(y, -1.0f, 1.0f);

            moveVector.x = x;
            moveVector.y = y;
        }


    }
}
