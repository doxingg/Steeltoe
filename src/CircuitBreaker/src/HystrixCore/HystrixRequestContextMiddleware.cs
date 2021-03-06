﻿// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Steeltoe.CircuitBreaker.Hystrix.Strategy.Concurrency;
using Steeltoe.CircuitBreaker.HystrixBase.Util;
using System.Threading.Tasks;

namespace Steeltoe.CircuitBreaker.Hystrix
{
    public class HystrixRequestContextMiddleware
    {
        private readonly RequestDelegate _next;

#pragma warning disable CS0618 // Type or member is obsolete
        public HystrixRequestContextMiddleware(RequestDelegate next, IApplicationLifetime applicationLifetime)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            _next = next;
            applicationLifetime.ApplicationStopping.Register(() => HystrixShutdown.ShutdownThreads());
        }

        public async Task Invoke(HttpContext context)
        {
            var hystrix = HystrixRequestContext.InitializeContext();

            await _next.Invoke(context).ConfigureAwait(false);

            hystrix.Dispose();
        }
    }
}
