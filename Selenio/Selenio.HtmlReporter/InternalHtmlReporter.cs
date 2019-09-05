using System;
using System.Drawing.Imaging;
using System.IO;
using System.Management;
using System.Reflection;
using System.Xml;

namespace Selenio.HtmlReporter
{
    internal class InternalHtmlReporter
    {
        public static string OverallErrorMessage = "";

        private static string previousTestClassName = "";
        private static string testMethodName;
        private static string testDescription = "TEST SUMMARY MISSING";
        private static string currentTestStep = "Test Step Missing";
        private static string previousTestStep = "";
        private static int testStepNumber = 0;

        private static string resultsDir = "";
        private static bool testResult = true;

        private static string testMethodResultFolder = "";
        private static string debugLogFullPath = "";
        private static string summaryLogFileName = "";
        private static string summaryLogFullPath = "";
        private static string debugLogFileName = "";

        public static DateTime ActionStartTime { get; set; }
        public static DateTime ActionEndTime { get; set; }

        private static DateTime testStartTime = new DateTime();
        private static DateTime testEndTime = new DateTime();

        ///<summary>
        ///<para>Test Description displayed in Reports.</para>
        ///<para>Example: Reports.TestDescription = "Verify order can be created in FAST Application";</para>
        ///</summary>
        public static string TestDescription
        {
            //get { return testDescription; }
            set { testDescription = value; }
        }

        ///<summary>
        ///<para>Test Step displayed in Reports.</para>
        ///<para>Example: Reports.TestStep = "Navigate to Quick File Entry Screen.";</para>
        ///</summary>
        public static string TestStep
        {
            //get { return currentTestStep; }
            set { currentTestStep = value; }
        }

        ///<summary>
        ///<para>Initializes the Test Before Run. This must be called in the TestInitialize method</para>
        ///<para>Example: Reports.InitTest(TestContext);</para>
        ///</summary>
        public static void Initialize(string methodName, string className, string resultsFolder, string assemblyDropDirectory)
        {
            OverallErrorMessage = "";
            testResult = true;
            testMethodName = methodName;
            debugLogFileName = $@"{testMethodName}_{DateTime.Now.ToString("MMddyyyyHHmmss")}.xml";
            testDescription = "TEST SUMMARY MISSING";
            currentTestStep = "Test Step Missing";
            testStepNumber = 0;
            previousTestStep = "";

            if (previousTestClassName != className)
            {
                previousTestClassName = className;
                resultsDir = resultsFolder;
                Directory.CreateDirectory(resultsDir);

                testMethodResultFolder = $@"{className}_{DateTime.Now.ToString("MMddyyyyHHmmss")}";
                resultsDir = $@"{resultsDir}\{testMethodResultFolder}";

                Directory.CreateDirectory(resultsDir);
                Directory.CreateDirectory(resultsDir + @"\Template");

                CreateSummaryLog();

                GetEmbeddedFile(assemblyDropDirectory, "Selenio.HtmlReporter", "Style.Summary.xsl", resultsDir + @"\Template\Summary.xsl");
                GetEmbeddedFile(assemblyDropDirectory, "Selenio.HtmlReporter", "Style.Details.xsl", resultsDir + @"\Template\Details.xsl");
                GetEmbeddedFile(assemblyDropDirectory, "Selenio.HtmlReporter", "Style.Style.css", resultsDir + @"\Template\Style.css");
            }
            else
            {
                Directory.CreateDirectory($@"{resultsDir}\Screenshots\{testMethodName}");
                debugLogFullPath = $@"{resultsDir}\{debugLogFileName}";
                CreateTestNode();
                CreateDebugLog();
            }
        }

        ///<summary>
        ///<para>Updates Result of Test Method in the Reports. This must be called as part of Test Clean up.</para>
        ///<para>Example: Reports.UpdateResult(TestContext);</para>
        ///</summary>
        public static TestExecutionOutcome EndTest(bool testPassed)
        {
            var outcome = new TestExecutionOutcome();

            if (testPassed && testResult)
                outcome.Passed = true;
            else if (!testResult)
                outcome.Passed = false;
            else
                outcome.Passed = testPassed;

            #region XML stuff
            XmlDocument doc = new XmlDocument();
            doc.Load(summaryLogFullPath);

            doc.SelectSingleNode("tests/codedui[@name='" + testMethodName + "']/status").InnerText = "complete";

            XmlNode XNode = doc.SelectSingleNode("/tests/codedui[@name='" + testMethodName + "']");

            XmlNode tResult = doc.CreateElement("result");
            tResult.InnerText = outcome.Passed ? "Passed" : "Failed";
            XNode.AppendChild(tResult);

            testEndTime = DateTime.Now;
            XmlNode eTime = doc.CreateElement("endtime");
            eTime.InnerText = testEndTime.ToString();
            XNode.AppendChild(eTime);

            XmlNode rTime = doc.CreateElement("runtime");
            rTime.InnerText = ((int)((testEndTime - testStartTime).TotalSeconds)).ToString();
            XNode.AppendChild(rTime);

            doc.Save(summaryLogFullPath);
            #endregion

            if (!outcome.Passed)
                outcome.Message = OverallErrorMessage == "" ? "See detailed report." : OverallErrorMessage;

            return outcome;
        }

        private static void CreateTestNode()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(summaryLogFullPath);

            XmlNode xNode = doc.GetElementsByTagName("tests")[0];

            XmlNode testNode = doc.CreateElement("codedui");
            XmlAttribute Valueattr = doc.CreateAttribute("name");
            Valueattr.Value = testMethodName;
            testNode.Attributes.Append(Valueattr);
            xNode.AppendChild(testNode);

            XmlNode stestid = doc.CreateElement("testid");
            stestid.InnerText = "-";
            testNode.AppendChild(stestid);

            XmlNode sDesc = doc.CreateElement("sDesc");
            sDesc.InnerText = "description";
            testNode.AppendChild(sDesc);

            testStartTime = DateTime.Now;
            XmlNode stTime = doc.CreateElement("starttime");
            stTime.InnerText = testStartTime.ToString();
            testNode.AppendChild(stTime);

            XmlNode sStatus = doc.CreateElement("status");
            sStatus.InnerText = "InProgress";
            testNode.AppendChild(sStatus);

            XmlNode sFile = doc.CreateElement("file");
            sFile.InnerText = debugLogFileName;
            testNode.AppendChild(sFile);

            doc.Save(summaryLogFullPath);
        }

        private static void CreateSummaryLog()
        {
            Directory.CreateDirectory(resultsDir + "\\Screenshots\\" + testMethodName);
            summaryLogFileName = "TESTSUMMARY_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".xml";
            summaryLogFullPath = resultsDir + "\\" + summaryLogFileName;

            XmlDocument doc = new XmlDocument();
            FileStream rfile = new FileStream(summaryLogFullPath, FileMode.Create);

            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlProcessingInstruction xProcess = doc.CreateProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"Template\\Summary.xsl\"");
            doc.AppendChild(xProcess);

            XmlNode productsNode = doc.CreateElement("tests");
            AddAttributes(productsNode, doc);
            doc.AppendChild(productsNode);

            XmlNode testNode = doc.CreateElement("codedui");
            XmlAttribute Valueattr = doc.CreateAttribute("name");
            Valueattr.Value = testMethodName;
            testNode.Attributes.Append(Valueattr);
            productsNode.AppendChild(testNode);

            XmlNode stestid = doc.CreateElement("testid");
            stestid.InnerText = "-";
            testNode.AppendChild(stestid);

            XmlNode sDesc = doc.CreateElement("sDesc");
            sDesc.InnerText = "description";
            testNode.AppendChild(sDesc);

            testStartTime = DateTime.Now;
            XmlNode stTime = doc.CreateElement("starttime");
            stTime.InnerText = testStartTime.ToString();
            testNode.AppendChild(stTime);

            XmlNode sStatus = doc.CreateElement("status");
            sStatus.InnerText = "InProgress";
            testNode.AppendChild(sStatus);

            XmlNode sFile = doc.CreateElement("file");
            sFile.InnerText = debugLogFileName;
            testNode.AppendChild(sFile);

            doc.Save(rfile);
            rfile.Close();
            debugLogFullPath = resultsDir + "\\" + debugLogFileName;
            CreateDebugLog();
        }

        private static void CreateDebugLog()
        {
            XmlDocument doc = new XmlDocument();
            FileStream reportFile = new FileStream(debugLogFullPath, FileMode.Create);

            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlProcessingInstruction xProcess = doc.CreateProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"Template\\Details.xsl\"");
            doc.AppendChild(xProcess);

            XmlNode testDetails = doc.CreateElement("testdetails");
            doc.AppendChild(testDetails);

            XmlNode testNode = doc.CreateElement("test");
            AddAttributes(testNode, doc);
            testDetails.AppendChild(testNode);

            doc.Save(reportFile);
            reportFile.Close();
        }

        public static void Fail(string errorMessage)
        {
            OverallErrorMessage = errorMessage;
            AppendActionToReport("Error Occured", "", "Fail", "", "", "", false, errorMessage);
            testResult = false;
        }

        public static void StatusUpdate(string message, bool result)
        {
            if (result)
                AppendActionToReport("", "", message, "", "", "", true, "");
            else
            {
                AppendActionToReport("", "", "", "", "", "", false, message);
                testResult = false;
            }
        }

        public static void ReportAction(string controlName, string action, string property, string value, bool result, string errorMessage)
        {
            AppendActionToReport(controlName, "", action, property, "", value, result, errorMessage);
        }

        public static void ReportAssertion<T>(string action, T expected, T actual, bool result, string errorMessage)
        {
            AppendActionToReport("", "", action, "", expected.ToString(), actual.ToString(), result, errorMessage);
        }

        public static void Log(string message, bool sectionStart = false, bool appendSectionBreak = false)
        {
            string lineStart = sectionStart ? "-----------------------" + Environment.NewLine : "";
            string lineEnd = Environment.NewLine + (appendSectionBreak ? "-----------------------" + Environment.NewLine : "");
            File.AppendAllText(resultsDir + @"\log.txt", $"{lineStart}{DateTime.Now.ToString()}: {message}{lineEnd}");
        }

        private static void AppendActionToReport(string controlName, string controlType, string action, string property, string expectedValue, string actualValue, bool result, string errorMessage)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(debugLogFullPath);

            XmlNode xNode = doc.SelectSingleNode("/testdetails/test[@name='" + testMethodName + "']");

            if (!xNode.HasChildNodes)
            {
                XmlNode descNode = doc.CreateElement("description");
                descNode.InnerText = testDescription;
                xNode.AppendChild(descNode);

                XmlDocument docSummary = new XmlDocument();
                docSummary.Load(summaryLogFullPath);

                XmlNode testNode = docSummary.DocumentElement.SelectSingleNode("/tests/codedui/sDesc");

                XmlNode parNode = testNode.ParentNode;
                parNode.RemoveChild(testNode);

                XmlNode sDesc = docSummary.CreateElement("description");
                sDesc.InnerText = testDescription;
                parNode.AppendChild(sDesc);

                docSummary.Save(summaryLogFullPath);
            }

            if (previousTestStep != currentTestStep)
            {
                currentTestStep = currentTestStep.Replace("'", "");
                testStepNumber = testStepNumber + 1;

                XmlNode tStep = doc.CreateElement("teststep");

                XmlAttribute teststepdescattr = doc.CreateAttribute("description");
                teststepdescattr.Value = testStepNumber + ". " + currentTestStep;
                XmlAttribute testStepNumberAttr = doc.CreateAttribute("stepnumber");
                testStepNumberAttr.Value = testStepNumber.ToString();

                previousTestStep = currentTestStep;

                tStep.Attributes.Append(teststepdescattr);
                tStep.Attributes.Append(testStepNumberAttr);

                xNode = xNode.AppendChild(tStep);
            }
            else
            {
                xNode = doc.SelectSingleNode("/testdetails/test[@name='" + testMethodName + "']/teststep[@description='" + testStepNumber + ". " + currentTestStep + "']");
            }

            XmlNode stmtNode = doc.CreateElement("dbg");

            XmlAttribute Controlattr = doc.CreateAttribute("Control");
            Controlattr.Value = controlName;

            XmlAttribute ControlTypeattr = doc.CreateAttribute("ControlType");
            ControlTypeattr.Value = controlType;

            XmlAttribute Methodattr = doc.CreateAttribute("Method");
            Methodattr.Value = action;

            XmlAttribute Propertyattr = doc.CreateAttribute("Property");
            Propertyattr.Value = property;

            XmlAttribute Valueattr = doc.CreateAttribute("Value");
            Valueattr.Value = expectedValue;

            XmlAttribute AValueattr = doc.CreateAttribute("AValue");
            AValueattr.Value = actualValue;

            XmlAttribute Resultattr = doc.CreateAttribute("Result");
            Resultattr.Value = result ? "Passed" : "Failed";

            XmlAttribute Errorattr = doc.CreateAttribute("Error");
            Errorattr.Value = errorMessage;

            XmlAttribute Screenattr = doc.CreateAttribute("Screenshot");
            Screenattr.Value = CaptureImage();

            ActionEndTime = DateTime.Now;
            XmlAttribute stepTimeattr = doc.CreateAttribute("stepTime");
            if (controlType != "")
                stepTimeattr.Value = ((int)((ActionEndTime - ActionStartTime).TotalSeconds)).ToString();
            else
                stepTimeattr.Value = "";

            stmtNode.Attributes.Append(Controlattr);
            stmtNode.Attributes.Append(ControlTypeattr);
            stmtNode.Attributes.Append(Methodattr);
            stmtNode.Attributes.Append(Propertyattr);
            stmtNode.Attributes.Append(Valueattr);
            stmtNode.Attributes.Append(AValueattr);
            stmtNode.Attributes.Append(Resultattr);
            stmtNode.Attributes.Append(Errorattr);
            stmtNode.Attributes.Append(stepTimeattr);
            stmtNode.Attributes.Append(Screenattr);

            xNode.AppendChild(stmtNode);

            doc.Save(debugLogFullPath);

            if (!result) testResult = false;
        }

        private static void AddAttributes(XmlNode testNode, XmlDocument doc)
        {
            // Test case name.
            XmlAttribute nameattr = doc.CreateAttribute("name");
            nameattr.Value = testMethodName;
            testNode.Attributes.Append(nameattr);

            // OS version.
            XmlAttribute osVersionAttribute = doc.CreateAttribute("osversion");
            osVersionAttribute.Value = GetOSFriendlyName();
            testNode.Attributes.Append(osVersionAttribute);

            // Browser name.
            XmlAttribute browserNameAttr = doc.CreateAttribute("browsername");
            browserNameAttr.Value = "-"; // browserName.ToString();
            testNode.Attributes.Append(browserNameAttr);

            // Run type (local vs MTM).
            XmlAttribute runtype = doc.CreateAttribute("runtype");
            runtype.InnerText = "Local run"; //runType
            testNode.Attributes.Append(runtype);

            // Agent name.
            XmlAttribute agentNameAttr = doc.CreateAttribute("agentname");
            agentNameAttr.InnerText = "-";
            testNode.Attributes.Append(agentNameAttr);

            // Build number (from build definition).
            XmlAttribute buildNumberAttr = doc.CreateAttribute("buildnumber");
            buildNumberAttr.InnerText = "-";
            testNode.Attributes.Append(buildNumberAttr);

            // Test plan name.
            XmlAttribute planNode = doc.CreateAttribute("testplan");
            planNode.InnerText = "-";
            testNode.Attributes.Append(planNode);

        }

        private static string CaptureImage()
        {
            string startTimeString = DateTime.Now.ToString().Replace('/', '_').Replace(':', '_').Replace(' ', '_');

            string screenshotPath = $@"{resultsDir}\Screenshots\{testMethodName}\{startTimeString}.jpeg";

            try
            {
                var image = ScreenCapture.CaptureDesktop();
                image.Save(screenshotPath, ImageFormat.Jpeg);
            }
            catch
            {
                // Just continue. Should report this to a log.
            }

            return screenshotPath;
        }

        private static string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }

        private static bool GetEmbeddedFile(string location, string binary, string file, string filePath)
        {
            string fileContents = string.Empty;

            try
            {
                //Assembly current = Assembly.LoadFrom($@"{location}\{binary}.dll");
                Assembly current = Assembly.LoadFrom(binary + ".dll");
                using (Stream stream = current.GetManifestResourceStream(binary + "." + file))
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        fileContents = streamReader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return false;
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(fileContents);
            }

            return true;
        }

    }
}
