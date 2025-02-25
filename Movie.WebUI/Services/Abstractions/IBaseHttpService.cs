﻿using Movie.WebUI.Models;

namespace Movie.WebUI.Services.Abstractions;

public interface IBaseHttpService
{
    public APIResponse ResponseModel { get; set; }
    public abstract Task<T> SendAsync<T>(ApiRequest apiRequest, bool isAuthenticated = false);
}
