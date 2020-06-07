
using System;
using System.Collections.Generic;

namespace FormatValidator
{
    internal interface IUserInterface
    {
        /// <summary>
        /// Display a start message
        /// </summary>
        public void ShowStart();

        /// <summary>
        /// Display an error for a reported row
        /// </summary>
        /// <param name="error"></param>
        public void ReportRowError(RowValidationError error);

        /// <summary>
        /// Show the summary details for the validation
        /// </summary>
        /// <param name="validator">The validator used</param>
        /// <param name="errors">The full list of errors</param>
        /// <param name="duration">The duration of the validation</param>
        public void ShowSummary(Validator validator, List<RowValidationError> errors, TimeSpan duration);
    }
}
