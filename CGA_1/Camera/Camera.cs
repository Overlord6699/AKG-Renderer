using CGA_1.Utils;
using System;
using System.Numerics;


namespace CGA_1.CameraInstances
{
    public class Camera : ICamera
    {
       
        public float Radius = .3f;

        public event EventHandler CameraChanged;


        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                if (PropertyChangedChecker.ValueChanged(ref _rotation, value))
                    CameraChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                if (PropertyChangedChecker.ValueChanged(ref _position, value))
                    CameraChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private Vector3 _position;
        private Quaternion _rotation = Quaternion.Identity;

        private float _squareRadius { get => Radius * Radius; }

        public Matrix4x4 ViewPort()
        {
            return Matrix4x4.CreateFromQuaternion(_rotation) //матрица поворота х матрица трансляции
              * Matrix4x4.CreateTranslation(_position);
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            if (PropertyChangedChecker.ValueChanged(ref this._rotation, rotation) || PropertyChangedChecker.ValueChanged(ref this._position, position))
            {
                CameraChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //гиперболическа проекция (вращение по сфере)
        internal Vector3 MapToSphere(Vector2 v)
        {

            var P = new Vector3(v.X, -v.Y, 0);

            var XY_squared = P.LengthSquared();

            if (XY_squared <= .5f * _squareRadius)
                P.Z = (float)Math.Sqrt(_squareRadius - XY_squared);  // теорема Пифагора
            else
                P.Z = 0.5f * _squareRadius / (float)Math.Sqrt(XY_squared);  // гиперболическое

            return Vector3.Normalize(P);
        }

        internal Quaternion CalculateQuaternion(Vector3 startV, Vector3 currentV)
        {

            var cross = Vector3.Cross(startV, currentV);

            if (cross.Length() > MathUtils.Epsilon) // если было вообще изменение какое-то (оптимизация)
                return new Quaternion(cross, Vector3.Dot(startV, currentV));
            else
                return Quaternion.Identity;
        }
        
    }
}
