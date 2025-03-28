﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Database.Repositories.Queries;

namespace Firefly.Server.Core.Database.Repositories.Queries.PostgreSQL
{
    internal class ConfigQueries : IConfigQueries
    {
        public string GetAll() => "SELECT jsonb_object_agg(Category, JsonData) FROM RemoteConfig";
    }
}
