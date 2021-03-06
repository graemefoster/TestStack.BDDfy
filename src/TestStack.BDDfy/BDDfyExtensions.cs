using System;
using System.Linq;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    public static class BDDfyExtensions
    {
        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <returns></returns>
        public static Story BDDfy(this object testObject, string scenarioTitle = null)
        {
            return testObject.LazyBDDfy(scenarioTitle).Run();
        }

        public static Engine LazyBDDfy(this object testObject, string scenarioTitle = null)
        {
            return InternalLazyBDDfy(testObject, scenarioTitle);
        }

        /// <summary>
        /// Extension method to BDDfy an object instance.
        /// </summary>
        /// <typeparam name="TStory">The type representing the story.</typeparam>
        /// <param name="testObject">The test object representing a scenario.</param>
        /// <param name="scenarioTitle">Overrides the default scenario title and is displayed in the reports.</param>
        /// <returns></returns>
        public static Story BDDfy<TStory>(this object testObject, string scenarioTitle = null)
            where TStory : class
        {
            return testObject.LazyBDDfy<TStory>(scenarioTitle).Run();
        }

        public static Engine LazyBDDfy<TStory>(this object testObject, string scenarioTitle = null)
            where TStory : class
        {
            return InternalLazyBDDfy(testObject, scenarioTitle, typeof(TStory));
        }

        static Engine InternalLazyBDDfy(
            object testObject, 
            string scenarioTitle, 
            Type explicitStoryType = null)
        {
            var testContext = TestContext.GetContext(testObject);

            var storyScanner = testContext.FluentScanner != null ?
                testContext.FluentScanner.GetScanner(scenarioTitle, explicitStoryType) :
                GetReflectiveScanner(testContext, scenarioTitle, explicitStoryType);

            return new Engine(storyScanner);
        }

        static IScanner GetReflectiveScanner(ITestContext testContext, string scenarioTitle = null, Type explicitStoryType = null)
        {
            var stepScanners = Configurator.Scanners.GetStepScanners(testContext).ToArray();
            var reflectiveScenarioScanner = new ReflectiveScenarioScanner(scenarioTitle, stepScanners);

            return new DefaultScanner(testContext, reflectiveScenarioScanner, explicitStoryType);
        }
    }
}