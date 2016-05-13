using FunctionalProgramming.Monad;
using NUnit.Framework;
using System;
using System.Linq;

namespace FunctionalProgramming.Tests.MonadTests
{
    [TestFixture]
    public class MaybeTests
    {
        [TestCase]
        public void TestMaybeNullable_NullValueIsNothing()
        {
            var result = default(int?).ToMaybe();
            Assert.IsTrue(result.IsEmpty);
        }

        [TestCase]
        public void TestMaybeNullable_NonNullValueIsSomething()
        {
            int? value = 1;
            var result = value.ToMaybe();
            Assert.IsFalse(result.IsEmpty);
        }

        [TestCase]
        public void TestMaybeString_EmptyStringIsNothing()
        {
            var result = string.Empty.ToMaybe();
            Assert.IsTrue(result.IsEmpty);
        }

        [TestCase]
        public void TestMaybeString_NullStringIsNothing()
        {
            var result = default(string).ToMaybe();
            Assert.IsTrue(result.IsEmpty);
        }

        [TestCase]
        public void TestMaybeString_WhitespaceStringIsNothing()
        {
            var result = " ".ToMaybe();
            Assert.IsTrue(result.IsEmpty);
        }

        [TestCase]
        public void TestMaybeString_NonNullOrWhitespaceStringIsSomething()
        {
            var result = "hello".ToMaybe();
            Assert.IsFalse(result.IsEmpty);
        }

        private static IMaybe<int> SafeDivide(int i)
        {
            return i % 2 == 0
                ? (i / 2).ToMaybe()
                : Maybe.Nothing<int>();
        }

        [Test]
        public void TestSelectJust()
        {
            var result = 5.ToMaybe().Select(n => n == 5).GetOrElse(() => false);
            Assert.IsTrue(result);
        }

        [Test]
        public void TestSelectManyNothingResult()
        {
            var expected = Maybe.Nothing<int>();
            var result = from a in SafeDivide(10)
                         from b in SafeDivide(a)
                         select b;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestSelectManyJustResult()
        {
            var expected = 5.ToMaybe();
            var result = from a in SafeDivide(20)
                         from b in SafeDivide(a)
                         select b;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestEqualityJust()
        {
            var expected = 5.ToMaybe();
            var result = 4.ToMaybe().Select(n => n + 1);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestInequalityJust()
        {
            var expected = 4.ToMaybe();
            var result = 4.ToMaybe().Select(n => n + 1);
            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void TestJustNothingInequality()
        {
            var expected = 4.ToMaybe();
            var result = Maybe.Nothing<int>();
            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void TestKeepSome()
        {
            var expected = new[] { 1, 2, 3, 4 };
            var test = new[]
            {
                1.ToMaybe()
            };

            var result = test.KeepSome();
            result.ToList().ForEach(Console.WriteLine);
        }
    }
}
