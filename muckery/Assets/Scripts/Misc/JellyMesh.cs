using UnityEngine;

public class JellyMesh : MonoBehaviour
{
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;

    Mesh _originalMesh;
    Mesh _meshClone;
    MeshRenderer _meshRenderer;

    JellyVertex[] _jellyVertices;
    Vector3[] _vertexArray;

    void Start()
    {
        _originalMesh = GetComponent<MeshFilter>().sharedMesh;

        _meshClone = Instantiate( _originalMesh );
        GetComponent<MeshFilter>().sharedMesh = _meshClone;

        _meshRenderer = GetComponent<MeshRenderer>();

        _jellyVertices = new JellyVertex[_meshClone.vertices.Length];

        for (int i = 0; i < _meshClone.vertices.Length; i++)
        {
            _jellyVertices[i] = new JellyVertex(
                i,
                transform.TransformPoint( _meshClone.vertices[i] )
            );
        }
    }

    void FixedUpdate()
    {
        _vertexArray = _originalMesh.vertices;

        for (int i = 0; i < _jellyVertices.Length; i++)
        {
            Vector3 target = transform.TransformPoint( _vertexArray[_jellyVertices[i].ID] );

            float intensity =
                (1 - (_meshRenderer.bounds.max.y - target.y) / _meshRenderer.bounds.size.y)
                * Intensity;

            _jellyVertices[i].Shake(target, Mass, stiffness, damping);

            target = transform.InverseTransformPoint(_jellyVertices[i].Position);

            _vertexArray[_jellyVertices[i].ID] =
                Vector3.Lerp(_vertexArray[_jellyVertices[i].ID], target, intensity);
        }

        _meshClone.vertices = _vertexArray;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;

        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;

            if ((velocity + Force + Force / m).magnitude < 0.001f)
                Position = target;
        }
    }
}