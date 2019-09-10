using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using log4net;

namespace Test.API.Framework
{
    public static class AssertManager
    {
        private static List<KeyValuePair<string, Expression<Action>>> assertions = new List<KeyValuePair<string, Expression<Action>>>();
        private static StringBuilder failedAssertions = new StringBuilder();
        private static StringBuilder passedAssertions = new StringBuilder();
        private static int failedCount = 0;
        private static int passedCount = 0;
        private static ILog log = LogManager.GetLogger(typeof(AssertManager));

        /// <summary>
        /// Execute all assertions in the list and clear the list.
        /// </summary>
        public static void Execute()
        {
            try
            {
                foreach (KeyValuePair<string, Expression<Action>> assertExp in assertions)
                {
                    try
                    {
                        assertExp.Value.Compile().Invoke();
                        passedCount++;
                        logPass(assertExp.Key);
                    }
                    catch (AssertFailedException ex)
                    {
                        failedCount++;
                        var expression = assertExp.ToString();
                        logFail(ex, expression, assertExp.Key);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("AssertManager Execute->Error: " + ex.Message);
            }
            finally
            {
                assertions.Clear();
            }
        }

        /// <summary>
        /// Execute managed assertion.
        /// </summary>
        /// <param name="newAssertion">() => Assert.Expression</param>
        public static void Execute(Expression<Action> newAssertion, string description = "")
        {
            add(newAssertion, description);
            Execute();
        }

        /// <summary>
        /// Execute all remaining assertions and validate no failures occurred during the current test.
        /// </summary>
        public static void ValidateTest()
        {
            Execute();

            try
            {
                log.Info(passedAssertions.ToString());
                Assert.AreEqual(0, failedCount, failedAssertions.ToString());
            }
            catch (AssertFailedException ex)
            {
                log.Error("ValidateTest Execute->Error: " + ex.Message);
                throw ex;
            }
            finally
            {
                clear();
            }
        }

        private static void add(Expression<Action> newAssertion, string description = "")
        {
            try
            {
                KeyValuePair<string, Expression<Action>> assert = new KeyValuePair<string, Expression<Action>>(description, newAssertion);
                assertions.Add(assert);
            }
            catch (Exception ex)
            {
                log.Error("AssertList Add->Error: " + ex.Message);
            }
        }

        private static void logFail(AssertFailedException ex, string expressionAsserted, string description)
        {
            if (failedAssertions.Length == 0)
            {
                failedAssertions.AppendLine("TOTAL FAILED ASSERTIONS");
                failedAssertions.AppendLine("---------------------------------------------------------------------------------------");
            }

            failedAssertions.AppendLine(string.Format("{0} {1}", ex.Message, description + ": FAILED"));
        }

        private static void logPass(string description)
        {
            if (passedAssertions.Length == 0)
            {
                passedAssertions.AppendLine("TOTAL PASSED ASSERTIONS");
                passedAssertions.AppendLine("---------------------------------------------------------------------------------------");
            }

            passedAssertions.AppendLine(description + ": PASSED");
        }

        private static void clear()
        {
            try
            {
                if (assertions.Count > 0)
                {
                    assertions.Clear();
                }

                if (failedAssertions.Length > 0)
                {
                    failedAssertions.Clear();
                }

                failedCount = 0;
                if (passedAssertions.Length > 0)
                {
                    passedAssertions.Clear();
                }

                passedCount = 0;
            }
            catch (Exception ex)
            {
                log.Error("AssertManager Clear->Error: " + ex.Message);
            }
        }


    }
}
