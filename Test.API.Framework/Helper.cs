
using System;
using System.Text;
using System.Threading;

namespace Test.API.Framework
{
    public class Helper
    {
        /// <summary>
        /// Generate random string using GUID.
        /// </summary>
        /// <param name="length">Length of the string to be generated. Not more than 32.</param>
        /// <returns>Random string.</returns>
        public static string GenerateRandomString(int length = 10)
        {
            Thread.Sleep(500);
            return Guid.NewGuid().ToString().ToLower().Substring(0, length).Replace('-', '_');
        }

        /// <summary>
        /// Generate random name.
        /// </summary>
        /// <param name="length">Length of the name.</param>
        /// <returns>Random name.</returns>
        public static string GenerateRandomName(int length = 10)
        {
            Thread.Sleep(500);

            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int chars = 0; chars < length; chars++)
            {
                sb.Append((char)random.Next(97, 123));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate random integer.
        /// </summary>
        /// <returns>Random integer.</returns>
        public static int GenerateRandomInteger()
        {
            Thread.Sleep(500);
            return new Random().Next(1, int.MaxValue);
        }
    }

    public enum Verb
    {
        [StringValue("GET")]
        GET,
        [StringValue("POST")]
        POST,
        [StringValue("DELETE")]
        DELETE
    }

    /// <summary>
    /// Class for specifying string values for Enum members.
    /// </summary>
    public class StringValue : Attribute
    {
        private readonly string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Get enum string value.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }
    }

    /// <summary>
    /// Class to get enum strings values.
    /// </summary>
    public static class StringEnum
    {
        /// <summary>
        /// Get string value
        /// </summary>
        /// <param name="value">Enum value.</param>
        /// <returns>Enum string.</returns>
        public static string GetStringValue(this Enum value)
        {
            string output = null;
            var type = value.GetType();

            var fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }

}
