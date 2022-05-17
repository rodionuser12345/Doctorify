﻿using Doctorify.Infrastructure.Data.Context;

namespace Doctorify.Middleware
{
    public class DbTransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public DbTransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, DoctorifyContext dbContext)
        {
            // For HTTP GET opening transaction is not required
            if (httpContext.Request.Method == HttpMethod.Get.Method)
            {
                await _next(httpContext);
                return;
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            await _next(httpContext);

            //Commit transaction if all commands succeed, transaction will auto-rollback when disposed if either commands fails
            await dbContext.Database.CommitTransactionAsync();
        }
        
    }
}
