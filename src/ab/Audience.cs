using System;

namespace ab
{
    public class Audience
    {
        public static Lazy<Func<string, bool>> SplitTwo = new Lazy<Func<string, bool>>(() => (identity =>
        {
            var or = identity.GetHashCode() % 2 == 0;
            return or;
        }));
    }
}