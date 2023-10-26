using Microsoft.AspNetCore.Builder;

namespace Classifieds.Auth;


public static class SwaggerHelpers
{
    public static void UseSwaggerRequestInterceptor(this WebApplication app)
    {
        app.UseSwaggerUI(c =>
        {
            c.UseRequestInterceptor(
                "(r)=>{if(!r.url.endsWith('/swagger.json'))" +
                "r.url=location.href.substring(0,location.href.indexOf('/swagger/'))+" +
                "r.url.replace(location.protocol+'//'+location.host,'');return r;}");
        });
    }
}