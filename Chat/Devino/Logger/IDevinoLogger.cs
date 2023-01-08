using Devino.Args;

namespace Devino.Logger
{
    public interface IDevinoLogger
    {
        void Log(string message);

        void LogApiRequest(ApiRequestEventArgs args);
        void LogApiResponse(ApiResponseEventArgs args);
        void LogApiRequestException(ApiRequestExceptionEventArgs args);
    }
}