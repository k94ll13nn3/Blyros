using System;

namespace Blyros.Tests
{
    public class FakeBuilder<T>
    {
        public Func<T, bool> Checker { get; set; }

        public void SetChecker(Func<T, bool> checker) => Checker = checker;
    }
}