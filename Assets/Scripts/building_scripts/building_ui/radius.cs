using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class radius : MonoBehaviour {


        [Range(0.1f, 100f)]
        public float radius_size = 10.0f;

        [Range(3, 256)]
         public int numSegments = 128;

        void Start ( ) {
            DoRenderer();
        }

        public void DoRenderer ( ) {
            

            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            
            Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Diffuse"));
            lineRenderer.startColor = c1; 
            lineRenderer.endColor = c1;
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.positionCount = numSegments + 1;
            

            float deltaTheta = (float) (2.0 * Mathf.PI) / numSegments;
            float theta = 0f;

            for (int i = 0 ; i < numSegments + 1 ; i++) {
                
                float x = radius_size * Mathf.Cos(theta);
                float z = radius_size * Mathf.Sin(theta);
            
                Vector3 pos = new Vector3(transform.position.x + x, 0.3f, transform.position.z + z); //Offset from world Origin
                
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
            
    }
}