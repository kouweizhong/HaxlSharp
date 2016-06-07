﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxlSharp.Internal
{
    public static class RunFetch
    {
        public static async Task<Scope> Run(HaxlFetch fetch, Scope scope, Func<IEnumerable<BlockedRequest>, Task> fetcher)
        {
            var result = fetch.Result.Value;
            return await result.Match(
                done => Task.FromResult(done.AddToScope(scope)),
                async blocked =>
                {
                    await fetcher(blocked.BlockedRequests);
                    return await Run(blocked.Continue, scope, fetcher);
                }
            );
        }
    }
}
