﻿namespace makeITeasy.AppFramework.Models
{
    public interface IBaseQuery
    {
        int? Take { get; set; }

        int? Skip { get; set; }

        bool IsPagingEnabled { get; set; }

        string SortBy { get; set; }

        bool SortDescending { get; set; }
    }
}