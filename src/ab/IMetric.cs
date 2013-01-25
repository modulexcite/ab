using System;

namespace ab
{
    public interface IMetric
    {
        string Name { get; set; }
        object[] GetValues(DateTime start, DateTime end);
    }
}