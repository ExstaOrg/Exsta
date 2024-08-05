// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Azure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NotificationFunction {
    public class SendNotification {
        private readonly ILogger<SendNotification> _logger;

        public SendNotification(ILogger<SendNotification> logger) {
            _logger = logger;
        }

        [Function(nameof(SendNotification))]
        public void Run([EventGridTrigger] CloudEvent cloudEvent) {
            _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);
        }
    }
}
