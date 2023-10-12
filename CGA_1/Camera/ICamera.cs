using System;
using System.Numerics;

namespace CGA_1.CameraInstances
{
    //закинуть свойства камеры?
    public interface ICamera
    {
        event EventHandler CameraChanged;
        Matrix4x4 ViewPort();
    }
}
