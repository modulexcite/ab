using System;

namespace ab
{
    // We might need deterministic hashes in a web farm, but the cohort itself isn't mission critical, so this should work as a default
    public class Audience
    {
        public static Lazy<Func<string, int, int>> Split = new Lazy<Func<string, int, int>>(() => (identity, n) => (int)(unchecked(((uint)identity.GetHashCode())) % n + 1));
    }
}