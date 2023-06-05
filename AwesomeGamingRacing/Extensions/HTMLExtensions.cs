using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace AwesomeGamingRacing.Extensions
{
    public static class HTMLExtensions
    {
        public static string ImageFileFor<TModel, TResult>(this IHtmlHelper helper, Expression<Func<TModel, TResult>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
