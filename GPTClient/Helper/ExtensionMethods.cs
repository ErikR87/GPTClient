﻿using System.Reflection;

namespace GPTClient.Helper;

public static class ExtensionMethods
{
    public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
    {
        dynamic awaitable = @this.Invoke(obj, parameters);
        await awaitable;
        return awaitable.GetAwaiter().GetResult();
    }
}