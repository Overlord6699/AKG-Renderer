using System;
using System.Drawing;
using System.Numerics;


namespace CGA_1.Utils
{
    //класс расширения
    public static class MathUtils
    {

        public const float PI_Deg = (float)Math.PI * 1f / 180f;
        public const float Epsilon = 1E-5f;

        public static float ToRad(this float angleDegree) => angleDegree * PI_Deg;

        public static Vector2 ToRad(this Vector2 v) => v * PI_Deg;

        //линейная интерполяция
        internal static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }

        public static float Clamp(this float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        //работа с векторами

        public static Vector2 ToVector2(this Vector3 p) => new Vector2(p.X, p.Y);

        public static Vector2 ToVector2(this Point p) => new Vector2(p.X, p.Y);

        public static Vector3 ToVector3(this Vector4 p) => new Vector3(p.X, p.Y, p.Z);

        public static void Distribute(this Vector3 p, out int x, out int y, out int z)
        {
            x = (int)p.X; y = (int)p.Y; z = (int)p.Z;
        }

        public static Vector3 ToNdc(this Vector4 p) => new Vector3(p.X, p.Y, p.Z) / (p.W == 0 ? MathUtils.Epsilon : p.W);
    }
}
