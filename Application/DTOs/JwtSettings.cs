﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Configuration settings for JWT.
    /// These values will typically be read from appsettings.json.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
    }
}
