namespace SimplerScore.Validators
{
    using Models;

    internal static class ValidatorBuilderHelper
    {
        /// <summary>
        /// Helper method for validating current provider with specified validator.
        /// </summary>
        /// <typeparam name="TValidatorBuilder">The type of the validator builder.</typeparam>
        /// <param name="curr">The curr.</param>
        /// <param name="obj">The object.</param>
        public static void ValidateWith<TValidatorBuilder> (ICurrentProvider curr, object obj = null)
            where TValidatorBuilder : IValidatorBuilder<ICurrentProvider>, new()
        {
            var builder = new TValidatorBuilder();
            var validator = builder.Build();

            validator.Validate(curr, obj);
        }
    }
}