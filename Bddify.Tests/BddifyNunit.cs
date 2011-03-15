using NUnit.Framework;

namespace Bddify.Tests
{
    public static class BddifyNunit
    {
        public static Bddifier Bddify(this object testObject)
        {
            var bddifier = new Bddifier(
                testObject,
                new GwtScanner(),
                new IProcessor[]
                { 
                    new TestRunner<InconclusiveException>(), 
                    new ConsoleReporter(),
                    new ExceptionHandler(Assert.Inconclusive)
                });

            bddifier.Run();
            return bddifier;
        }
    }
}