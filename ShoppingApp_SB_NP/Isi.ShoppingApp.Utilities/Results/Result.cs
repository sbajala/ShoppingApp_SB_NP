using System;

namespace Isi.Utility.Results
{
    /// <summary>
    /// A Result object represents the result of an operation that could succeed or fail.
    /// </summary>
    /// <remarks>
    /// Every result must either be:
    /// <list type="number">
    /// <item>Successful, with a null error message, or</item>
    /// <item>Not successful, with a non-null error message.</item>
    /// </list>
    /// Use the static methods Success and Error to create Result objects.
    /// </remarks>
    public class Result
    {
        private static readonly Result success = new Result();

        public bool Successful { get; }
        public bool Failed => !Successful;

        public string ErrorMessage { get; }

        /// <summary>
        /// Constructor for creating Success results.
        /// </summary>
        protected Result()
        {
            Successful = true;
            ErrorMessage = null;
        }

        /// <summary>
        /// Constructor for creating Error results.
        /// </summary>
        protected Result(string errorMessage)
        {
            ValidateErrorMessage(errorMessage);

            Successful = false;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return (Successful) ? nameof(Successful) : $"Error: {ErrorMessage}";
        }

        /// <summary>
        /// Returns a Result object that represents a successful operation.
        /// </summary>
        /// <returns>A Result object where Successful is true and ErrorMessage is null.</returns>
        public static Result Success()
        {
            return success;
        }

        /// <summary>
        /// Returns a Result object that represents an error and contains an error message.
        /// </summary>
        /// <param name="errorMessage">Error message must not be null.</param>
        /// <returns>A Result object where Successful is false and ErrorMessage is non-null.</returns>
        /// <exception cref="ArgumentNullException">Error message must not be null.</exception>
        public static Result Error(string errorMessage)
        {
            return new Result(errorMessage);
        }

        protected static void ValidateErrorMessage(string errorMessage)
        {
            if (errorMessage == null)
                throw new ArgumentNullException(nameof(errorMessage), "Error message must not be null.");
        }
    }
}
