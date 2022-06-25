using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;
        private readonly ILogger<TRequest> logger;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators,
            ILogger<TRequest> logger)
        {
            this.validators = validators;
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            //extract request info
            string requestName = typeof(TRequest).Name;
            string unqiueId = Guid.NewGuid().ToString();
            string requestJson = JsonSerializer.Serialize(request);
            try
            {

                //check for validation request
                var context = new ValidationContext<TRequest>(request);
                var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResult.SelectMany(s => s.Errors).Where(w => w != null).ToList();
                if (failures.Any())
                {
                    logger.LogError($"validation error Request Id:{unqiueId}, request name:{requestName}, request json:{requestJson}, " +
                        $"validation errors json:{JsonSerializer.Serialize(failures)}");
                    throw new ValidationException(failures);
                }

                logger.LogInformation($"Begin Request Id:{unqiueId}, request name:{requestName}, request json:{requestJson}");

                //get the duration of request
                var timer = new Stopwatch();
                timer.Start();
                var response = await next();
                timer.Stop();

                logger.LogInformation($"End Request Id:{unqiueId}, request name:{requestName}, total request time:{timer.ElapsedMilliseconds}ms");

                return response;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Unhandled Exception Request Id:{unqiueId}, request name:{requestName}, request json:{requestJson}," +
                    $"Exception message:{ex.Message}");
                throw;
            }
        }
    }
}
