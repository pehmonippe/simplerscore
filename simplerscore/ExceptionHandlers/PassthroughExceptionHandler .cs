namespace SimplerScore.ExceptionHandlers
{
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.ExceptionHandling;
    using JetBrains.Annotations;

    public class PassthroughExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync([NotNull] ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var info = ExceptionDispatchInfo.Capture(context.Exception);
            info.Throw();
            return Task.Delay(10, cancellationToken);
        }
    }
}