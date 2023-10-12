namespace CGA_1
{
    class PropertyChangedChecker
    {

        //проверка и перезапись удобная
        public static bool ValueChanged<T>(ref T oldValue, T newValue)
        {
            if (object.Equals(oldValue, newValue))
                return false;

            oldValue = newValue;

            return true;
        }
    }
}
