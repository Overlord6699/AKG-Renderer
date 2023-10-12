using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace CGA_1.Utils
{
    static class ControlsUtils
    {

        public static Vector2 NormalizePointToClient(this Control control, Point position)
        {
            //учитываем центр экраан
            return new Vector2(
                position.X * (2f / control.Width) - 1.0f,
                position.Y * (2f / control.Height) - 1.0f);
        }

        //оказывается, нужно кнопки не попутать
        public static void GetMouseControls(out bool left, out bool right)
        {
            left = Control.MouseButtons.HasFlag(MouseButtons.Left);
            right = Control.MouseButtons.HasFlag(MouseButtons.Right);
        }
    }
}
