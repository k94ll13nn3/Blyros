using System;

namespace NameInProgress.Tests
{
    public class FakeBuilder
    {
        public Func<string, bool> Checker { get; set; }

        public void SetChecker(Func<string, bool> checker) => Checker = checker;
    }
}