﻿using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ExecutionError
    {
        [JsonProperty("code")]
        public ExecutionErrorCode Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public enum ExecutionErrorCode
    {
        INVALID_CREDENTIALS,
        INVALID_CREDENTIALS_MFA,
        ALREADY_LOGGED_IN,
        ACCOUNT_LOCKED,
        SITE_NOT_AVAILABLE,
        UNEXPECTED_ERROR
    }
}
