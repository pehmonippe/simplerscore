namespace SimplerScore.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using DataObjects;
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

        public static List<ScheduleEntry> ToScheduledOrder ([NotNull] this MeetModel meetModel)
        {
            var ids = meetModel.Events
                .Select(tp => tp.Id)
                .ToList();

            var scheduledIds = meetModel.TimePoints
                .OrderBy(tp => tp.Time)
                .SelectMany(tp => tp.EventIds)
                .ToList();

            var unscheduledIds = ids
                .Except(scheduledIds)
                .ToList();

            var unscheduledEventsEntry = new ScheduleEntry
            {
                Time = meetModel.DateOfEvent.TimeOfDay,
                Behavior = SchedulingBehavior.Unspecified,
                Events = meetModel.Events
                    .Where(e => unscheduledIds.Contains(e.Id))
                    .Select(e => e.Name)
                    .ToList()
            };

            var schedule = new List<ScheduleEntry> { unscheduledEventsEntry };

            var scheduledEvents = meetModel.TimePoints
                .OrderBy(tp => tp.Time)
                .Select(tp => new ScheduleEntry
                    {
                        Time = tp.Time,
                        Behavior = tp.Behavior,
                        Events = tp.EventIds
                            .Select(id => meetModel[id].Name)
                            .ToList()
                    })
                .ToList();

            schedule.AddRange(scheduledEvents);
            return schedule;
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