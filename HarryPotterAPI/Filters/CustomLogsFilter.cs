using HarryPotterAPI.Models;
using HarryPotterAPI.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using HarryPotterAPI.Logs;

namespace HarryPotterAPI.Filters
{
    public class CustomLogsFilter : IResultFilter, IActionFilter
    {
        private readonly List<int> _sucessStatusCodes;
        private readonly ICharacterRepository _repository;
        private readonly Dictionary<int, Character> _contextDict;

        public CustomLogsFilter(ICharacterRepository repository)
        {           
            _repository = repository;
            _contextDict = new Dictionary<int, Character>();
            _sucessStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created, StatusCodes.Status204NoContent};
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "Character", StringComparison.InvariantCultureIgnoreCase))
            {
                int id = 0;
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out id))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var character = _repository.GetCharacterById(id);
                        if (character != null)
                        {
                            var gameClone = character.clone();
                            _contextDict.Add(id, gameClone);
                        }
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/Character", StringComparison.InvariantCulture))
            {
                if (_sucessStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    int.TryParse(context.HttpContext.Request.Path.ToString().Split("/").Last(), out int id);
                    
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var afterUpdate = _repository.GetCharacterById(id);
                        if (afterUpdate != null)
                        {
                            Character beforeUpdate;
                            if (_contextDict.TryGetValue(id, out beforeUpdate))
                            {
                                CustomLogs.SaveLog("Character", afterUpdate.Id, afterUpdate.Name, context.HttpContext.Request.Method, beforeUpdate, afterUpdate);
                                _contextDict.Remove(id);
                            }
                        }
                    }
                    else if (context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Character beforeUpdate;
                        if (_contextDict.TryGetValue(id, out beforeUpdate))
                        {
                            CustomLogs.SaveLog("Character", beforeUpdate.Id, beforeUpdate.Name, context.HttpContext.Request.Method);
                            _contextDict.Remove(id);
                        }
                    }
                }
            }
        }

        #region Unused
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

        }
        #endregion
    }
}
