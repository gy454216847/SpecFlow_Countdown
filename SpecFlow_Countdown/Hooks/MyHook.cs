using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;

using SpecFlow_Countdown.Core;

using System.Reflection;

using TechTalk.SpecFlow;

namespace SpecFlow_Countdown.Hooks
{
    [Binding]
    public sealed class MyHook
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        private static ExtentTest feature;
        private static ExtentTest scenario;
        private static ExtentReports extent;

        [BeforeTestRun]
        public static void InitializeReport()
        {
            string reportFile = CommonWeb.GetCurrentTime() + "/";
            //Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter(CommonWeb.CurPath+"Reports//"+reportFile);

            htmlReporter.LoadConfig(CommonWeb.CurPath + "Config/report-config.xml");
            extent = new ExtentReports();

            extent.AttachReporter(htmlReporter);
        }
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            //Create dynamic feature name
            feature = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
            feature.AssignAuthor("Garry");
        }

        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            scenario = feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            //TODO: implement logic that has to run before executing each scenario
            CommonWeb.Open("chrome", "https://www.countdown.co.nz/");
            CommonWeb.MaxWindow();
        }

        [AfterStep]
        public static void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getter = pInfo.GetGetMethod(nonPublic: true);
            object TestResult = getter.Invoke(scenarioContext, null);

            //object TestResult = typeof(ScenarioContext).GetProperty("TestStatus", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(nonPublic: true).Invoke(scenarioContext, null);

            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "And")
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
            }
            if (scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message).AddScreenCaptureFromPath(CommonWeb.GetScreenShot("error"), CommonWeb.GetCurrentTime() + "_error");
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message).AddScreenCaptureFromPath(CommonWeb.GetScreenShot("error"), CommonWeb.GetCurrentTime() + "_error");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message).AddScreenCaptureFromPath(CommonWeb.GetScreenShot("error"), CommonWeb.GetCurrentTime() + "_error");
                else if (stepType == "And")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message).AddScreenCaptureFromPath(CommonWeb.GetScreenShot("error"), CommonWeb.GetCurrentTime() + "_error");
            }

            //Pending Status
            //if (TestResult.ToString() == "Step Definition Pending")
            //{
            //    if (stepType == "Given")
            //        scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
            //    else if (stepType == "When")
            //        scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
            //    else if (stepType == "Then")
            //        scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
            //}
        }

        [AfterScenario]
        public static void AfterScenario(ScenarioContext scenarioContext)
        {
            //TODO: implement logic that has to run after executing each scenario
            //    if (scenarioContext.TestError != null)
            //    {
            //        CommonWeb.GetScreenShot("error");
            //        scenario.Fail("details").AddScreenCaptureFromPath(CommonWeb.GetScreenShot("error"), "error");
            //    }
            CommonWeb.Quit();
        }
        [AfterTestRun]
        public static void TearDownReport()
        {
            extent.Flush();
        }
    }
}