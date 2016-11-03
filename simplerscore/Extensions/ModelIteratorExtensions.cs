namespace SimplerScore.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Models;

    internal static class ModelIteratorExtensions
    {
        public static IEnumerable<AthleteModel> ToRunningOrder ([NotNull, ItemNotNull] this IEnumerable<AthleteModel> athletes)
        {
            var sortedAthletes = athletes
                .OrderBy(a => a.RunningOrder)
                .ToList();

            return sortedAthletes;
        }

        public static IEnumerable<EventModel> ToScheduledOrder ([NotNull, ItemNotNull] this IEnumerable<EventModel> events)
        {
            var sortedEvents = events
                .OrderBy(e => e.ScheduledTime)
                .ThenBy(e => e.Group)
                .ThenBy(e => e.Order)
                .ToList();

            return sortedEvents;
        }

        public static TModel GetFollowingItem<TModel> ([CanBeNull] this TModel current, [NotNull, ItemNotNull] IEnumerable<TModel> sortedItems)
            where TModel : class, IModel
        {
            var model = current.GetSubsequentItem(sortedItems);
            return model;
        }

        public static TModel GetPreviousItem<TModel> ([CanBeNull] this TModel current, [NotNull, ItemNotNull] IEnumerable<TModel> sortedItems)
            where TModel : class, IModel
        {
            var sortedEvents = sortedItems.Reverse();
            var model = current.GetSubsequentItem(sortedEvents);

            return model;
        }

        /// <summary>
        /// Gets the subsequent event from a (sorted) .
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="sortedItems">The sorted events.</param>
        /// <returns></returns>
        private static TModel GetSubsequentItem<TModel> ([CanBeNull] this TModel current, [NotNull, ItemNotNull] IEnumerable<TModel> sortedItems)
            where TModel : class, IModel
        {
            // if we do not have current, then take the first
            if (null == current)
                return sortedItems.FirstOrDefault();

            // look for next following current.
            TModel model = null;
            var takeNext = false;

            // find current and take the next
            foreach (var @event in sortedItems)
            {
                if (takeNext)
                {
                    model = @event;
                    break;
                }

                if (@event.Id != current.Id)
                    continue;

                takeNext = true;
            }

            return model;
        }
    }
}