namespace SimplerScore.ModelBinders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using DataObjects;
    using JetBrains.Annotations;

    internal static class TimePointModelBinderExtensions
    {
        private static string GetValue ([NotNull] this ModelBindingContext context, [NotNull] string key)
        {
            //var hasPrefix = context.ValueProvider.ContainsPrefix(context.ModelName);

            //var searchPrefix = hasPrefix ?
            //    context.Model + "." :
            //    string.Empty;

            //var result = context.ValueProvider.GetValue(searchPrefix + key);
            var result = context.ValueProvider.GetValue(key);
            return result?.AttemptedValue;
        }

        private static T GetParameterValue<T> (this string input, [CanBeNull] T defaultValue, [NotNull] Func<string, T> parseFunc)
        {
            T result;

            try
            {
                result = parseFunc(input);
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        public static T GetRequestParameterValue<T> ([NotNull] this ModelBindingContext context, [NotNull] string key, [CanBeNull] T defaultValue, [NotNull] Func<string, T> parseFunc)
        {
            var value = context.GetValue(key);
            var result = value.GetParameterValue(defaultValue, parseFunc);

            return result;
        }

        public static List<T> RequestParameterValueAsList<T> ([NotNull] this ModelBindingContext context, [NotNull] string propertyName, [CanBeNull] T defaultValue, [NotNull] Func<string, T> parseFunc)
        {
            var value = context.GetValue(propertyName);

            if (string.IsNullOrWhiteSpace(value))
                return new List<T>();

            var items = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => GetParameterValue(item.Trim(), defaultValue, parseFunc))
                .ToList();

            return items;
        } 
    }

    internal class TimePointModelBinder : IModelBinder
    {
        public bool BindModel (HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (typeof (TimePoint) != bindingContext.ModelType)
                return false;

            var timePoint = (TimePoint) bindingContext.Model ?? new TimePoint();

            timePoint.MeetId = bindingContext.GetRequestParameterValue("meet", 0, int.Parse);
            timePoint.EventIds = bindingContext.RequestParameterValueAsList("events", -1, int.Parse)
                .Where(id => id != -1)
                .ToList();
            timePoint.Time = bindingContext.GetRequestParameterValue("time", TimeSpan.Zero, TimeSpan.Parse);
            timePoint.Behavior = bindingContext.GetRequestParameterValue("behavior", SchedulingBehavior.Exact, BehaviorParse);
            timePoint.Interlaced = bindingContext.GetRequestParameterValue("interlaced", true, bool.Parse);
            
            bindingContext.Model = timePoint;
            return true;
        }

        #region Private methods        
        /// <summary>
        /// Helper method to parse optional date value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static TimeSpan TimeOfDayParse (string input)
        {
            return TimeSpan.Parse(input);
        }

        /// <summary>
        /// Helper method to parse 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static SchedulingBehavior BehaviorParse (string input)
        {
            return (SchedulingBehavior) Enum.Parse(typeof (SchedulingBehavior), input);
        }

        #endregion
    }
}
