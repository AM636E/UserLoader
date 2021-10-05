using System;

namespace UserLoader.Mq
{
    public static class Helpers
    {
        public static T DoubleCheckLock<T>(object @lock, Func<bool> check, Func<T> producer, Func<T> item)
        {
            if (!check()) return item();
            lock (@lock)
            {
                return check() ? producer() : item();
            }
        }
    }
}