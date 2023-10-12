using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;
using CGA_1.CameraInstances;
using CGA_1.Utils;

namespace CGA_1.ControlsHandlers
{

    public class CameraControlHandler
    {
        public Camera Camera
        {
            get => _camera;
            set => PropertyChangedChecker.ValueChanged(ref _camera, value);
        }

        public Control Control
        {
            get => _control;
            set
            {
                var oldControl = _control;

                if (PropertyChangedChecker.ValueChanged(ref _control, value))
                {
                    //переподписка
                    if (oldControl != null)
                    {
                        oldControl.MouseDown -= Control_MouseDown;
                        oldControl.MouseMove -= Control_MouseMove;
                        _control.MouseUp -= Control_MouseUp;
                    }

                    if (_control != null)
                    {
                        _control.MouseDown += Control_MouseDown;
                        _control.MouseMove += Control_MouseMove;
                        _control.MouseUp += Control_MouseUp;
                    }
                }
            }
        }

        private Camera _camera;
        private Control _control;

        private Point _prevMousePosition;
        private Vector3 _prevCameraPosition;
        private Quaternion _prevCameraRotation;

        private float _yMultiplier = 10f;



        public CameraControlHandler(Control control, Camera camera)
        {
            Control = control;
            _camera = camera;
        }


        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            _leftMouseButton = false;
            _rightMouseButton = false;
            _control.Cursor = Cursors.Default;
        }

        private bool _rightMouseButton;
        private bool _leftMouseButton;

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            ControlsUtils.GetMouseControls(out _leftMouseButton, out _rightMouseButton);
            _prevMousePosition = e.Location;

            if (_leftMouseButton && _rightMouseButton)
            {
                _prevCameraPosition = _camera.Position;
                _control.Cursor = Cursors.SizeNS;
            }
            else if (_leftMouseButton)
            {
                _prevCameraRotation = _camera.Rotation;
                _control.Cursor = Cursors.NoMove2D;
            }
            else if (_rightMouseButton)
            {
                _prevCameraPosition = _camera.Position;
                _control.Cursor = Cursors.SizeAll;
            }
        }

        void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (_leftMouseButton && _rightMouseButton)
            {
                var deltaY = _prevMousePosition.Y - e.Location.Y;
                _camera.Position = _prevCameraPosition + new Vector3(0, 0, deltaY / _yMultiplier);
            }
            else if (_leftMouseButton)
            {
                var oldNpc = _control.NormalizePointToClient(_prevMousePosition);
                var oldVector = _camera.MapToSphere(oldNpc);

                var curNpc = _control.NormalizePointToClient(e.Location);
                var curVector = _camera.MapToSphere(curNpc);

                var deltaRotation = _camera.CalculateQuaternion(oldVector, curVector);
                _camera.Rotation = deltaRotation * _prevCameraRotation;
            }
            else if (_rightMouseButton)
            {
                var deltaPosition = new Vector3(e.Location.ToVector2() - _prevMousePosition.ToVector2(), 0);
                _camera.Position = _prevCameraPosition + (deltaPosition * new Vector3(1, -1, 1)) / 100;
            }
        }
    }

}
