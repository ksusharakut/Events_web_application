﻿namespace Application.Common
{
    public static class FileConstants
    {
        public const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    }
}
