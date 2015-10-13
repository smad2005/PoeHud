using System;

namespace PoeHUD.Framework.Helpers
{
    public static class ActionHelper
    {
        public static void SafeInvoke<T>(this Action<T> action, T parameter)
        {
            if (action != null)
            {
                action(parameter);
            }
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 parameter1, T2 parameter2, T3 parameter3)
        {
            if (action != null)
            {
                action(parameter1, parameter2, parameter3);
            }
        }

        public static void SafeInvoke(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        public static void ThrowIf<TException>(bool condition)
            where TException : Exception, new()
        {
            if (condition)
            {
                throw new TException();
            }
        }

        public static bool TryInvoke(this Action action)
        {
            try
            {
                action?.Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}