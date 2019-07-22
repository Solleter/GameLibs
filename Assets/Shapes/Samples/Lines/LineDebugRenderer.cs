using UnityEngine;
using System.Collections.Generic;

namespace Shapes.Samples.Lines
{
	public class LineDebugRenderer : MonoBehaviour
	{
		public Transform startPointTransform;
		public Transform endPointTransform;

		public LineInfo lineInfo;

        private List<Vector3> points = new List<Vector3>()
        {
            new Vector3(0, 0, 0),
            new Vector3(2, 0, 0),
            new Vector3(2, 2, 0),
            new Vector3(0, 2, 0),
            new Vector3(0, 0, 0)
        };
        

        private void Start()
        {
           
        }

        private void Update()
		{
            //Rendering();
            DrawPolygon();
		}

        private void Rendering(Vector3 start, Vector3 end)
        {
            //if (startPointTransform == null || endPointTransform == null) return;

            //lineInfo.startPos = startPointTransform.position;
            //lineInfo.endPos = endPointTransform.position;
            lineInfo.startPos = start;
            lineInfo.endPos = end;

            lineInfo.forward = -Camera.main.transform.forward;

            LineSegment.Draw(lineInfo);
        }

        private void DrawPolygon()
        {
            for(int i = 0; i < points.Count - 1; ++i)
            {
                Vector3 start = points[i];
                Vector3 end = points[i + 1];

                Rendering(start, end);
            }
        }
        
    }
}
