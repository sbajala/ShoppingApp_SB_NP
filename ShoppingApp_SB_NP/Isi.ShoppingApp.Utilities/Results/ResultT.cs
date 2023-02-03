using System;

namespace Isi.Utility.Results
{
    /// <summary>
    /// A Result&lt;<typeparamref name="T"/>&gt; object represents the result of an operation
    /// that could either successfully return a <typeparamref name="T"/> or else fail to do so.
    /// </summary>
    /// <remarks>
    /// Every result must either be:
    /// <list type="number">
    /// <item>Successful, with a null error message, and with resulting data of type <typeparamref name="T"/>, or</item>
    /// <item>Not successful, with a non-null error message, and with no data (default/null data).</item>
    /// </list>
    /// Use the static methods Success and Error to create Result&lt;<typeparamref name="T"/>&gt; objects.
    /// </remarks>
    public class Result<T> : Result
    {
        public T Data { get; }

        /// <summary>
        /// Constructor for creating Success results.
        /// </summary>
        protected Result(T data)
            : base()
        {
            Data = data;
        }

        /// <summary>
        /// Constructor for creating Error results.
        /// </summary>
        protected Result(string errorMessage)
            : base(errorMessage)
        {
            Data = default;
        }

        public override string ToString()
        {
            return (Successful) ? $"{nameof(Successful)}: {Data}" : $"Error: {ErrorMessage}";
        }

        /// <summary>
        /// Returns a Result&lt;<typeparamref name="T"/>&gt; object
        /// that represents a successful operation with valid resulting data.
        /// </summary>
        /// <returns>
        /// A Result&lt;<typeparamref name="T"/>&gt; object where Successful is true,
        /// ErrorMessage is null, and Data stores the valid resulting data.
        /// </returns>
        public static Result<T> Success(T data)
        {
            return new Result<T>(data);
        }

        /// <summary>
        /// Returns a Result&lt;<typeparamref name="T"/>&gt; object
        /// that represents an error and contains an error message.
        /// </summary>
        /// <param name="errorMessage"> Error message must not be null. </param>
        /// <returns>
        /// A Result&lt;<typeparamref name="T"/>&gt; object where Successful is false,
        /// ErrorMessage is non-null, and Data is default/null.
        /// </returns>
        /// <exception cref="ArgumentNullException"> Error message must not be null. </exception>
        public new static Result<T> Error(string errorMessage)
        {
            return new Result<T>(errorMessage);
        }
    }
}
