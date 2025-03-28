﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entities.RemoteConfig
{
    public class IRCConfig : IConfig
    {
        public required int Port { get; set; }
        public required bool Enabled { get; set; }

        public IPAddress? IP { get; set; }

        public string? MOTD { get; set; }

        public bool Validate(ref List<string> messages) {
            bool validationPassed = true;

            if (Port < Constants.LOWEST_PORT_ALLOWED || Port > Constants.HIGHEST_PORT_ALLOWED) {
                messages.Add($"IRC Port must be no lower than {Constants.LOWEST_PORT_ALLOWED} and no higher than {Constants.HIGHEST_PORT_ALLOWED}. Current value is {Port}");
                validationPassed = false;
            }

            return validationPassed;
        }
    }
}
