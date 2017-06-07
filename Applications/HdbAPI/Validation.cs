using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HdbApi.Validation
{
    public class Validator
    {
        /// <summary>
        /// Check API input by attempting to convert to a type
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="input"></param>
        /// <param name="targetType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public ValidationResult ValidateInput(string inputName, string input, Type targetType, ValidationResult result)
        {
            var tempValidation = new ValidationResult();
            if (input == null)
            {
                tempValidation.IsValid = false;
                tempValidation.ValidationMessage = inputName + " cannot be unassigned.";
            }
            else
            {
                try
                {
                    TypeDescriptor.GetConverter(targetType).ConvertFromString(input);
                    tempValidation.IsValid = true;
                }
                catch
                {
                    tempValidation.IsValid = false;
                    tempValidation.ValidationMessage = input + " is not a valid " + targetType.ToString() + ". ";
                }
            }

            if (tempValidation.IsValid == false)
            {
                result.IsValid = false;
                result.ValidationMessage += tempValidation.ValidationMessage;                
                return result;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Check API input using a predefined List
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="input"></param>
        /// <param name="targetList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public ValidationResult ValidateInput(string inputName, string input, List<string> targetList, ValidationResult result)
        {
            var tempValidation = new ValidationResult();
            if (input == null)
            {
                tempValidation.IsValid = false;
                tempValidation.ValidationMessage = inputName + " cannot be unassigned. ";
            }
            else
            {
                if (targetList.Contains(input))
                {
                    tempValidation.IsValid = true;
                }
                else
                {
                    tempValidation.IsValid = false;
                    tempValidation.ValidationMessage = input + " is not a valid option in {" + string.Join(",", targetList) + "}. ";

                }
            }

            if (tempValidation.IsValid == false)
            {
                result.IsValid = false;
                result.ValidationMessage += tempValidation.ValidationMessage;                
                return result;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Check API input using Regular Expressions
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="input"></param>
        /// <param name="regexCheck"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public ValidationResult ValidateInput(string inputName, string input, string regexCheck, ValidationResult result)
        {
            var tempValidation = new ValidationResult();
            if (input == null)
            {
                tempValidation.IsValid = false;
                tempValidation.ValidationMessage = inputName + " cannot be unassigned. ";
            }
            else
            {
                Regex regex = new Regex(@"^.*$");
                if (regexCheck == "allnumbers")
                {
                    regex = new Regex(@"^-?[\d]?[,\d,]*[\d]$");
                }
                Match match = regex.Match(input);
                if (match.Success)
                {
                    tempValidation.IsValid = true;
                }
                else
                {
                    tempValidation.IsValid = false;
                    tempValidation.ValidationMessage = input + " failed the " + regexCheck + " check. ";

                }
            }

            if (tempValidation.IsValid == false)
            {
                result.IsValid = false;
                result.ValidationMessage += tempValidation.ValidationMessage;
                return result;
            }
            else
            {
                return result;
            }
        }

    }

    /// <summary>
    /// Class for validation results
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public string ValidationMessage { get; set; } = null;
    }
}
