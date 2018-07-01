using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MethodStore.EF
{
    internal class Context<T> where T : class
    {
        internal List<Models.Method> GetListMethods(ParametersSearch parametersSearch)
        {
            List<Models.Method> methods;

            using (MethodStoreContext context = new MethodStoreContext())
            {
                DbSet<Models.Method> contextMethods = context.Methods;

                IQueryable<Models.Method> contextMethodsSearch = null;

                if (!string.IsNullOrWhiteSpace(parametersSearch.Text))
                {
                    string searchText = parametersSearch.Text;

                    contextMethodsSearch = contextMethods.Where(f =>
                        parametersSearch.SearchInGroup && f.Group.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || parametersSearch.SearchInType && f.Type.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || parametersSearch.SearchInObjectName && f.ObjectName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        || parametersSearch.SearchInMethodName && f.MethodName.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                }

                if (contextMethodsSearch == null)
                    contextMethodsSearch = contextMethods.Where(f => true);

                contextMethodsSearch?.OrderByDescending(f => f.ID);
                methods = contextMethodsSearch?.ToList();
            }
            return methods;
        }

        internal void RemoveMethods(T method)
        {
            using (MethodStoreContext context = new MethodStoreContext())
            {
                context.Remove(method);
                context.SaveChanges();
            }
        }

        internal T FindByID(int id)
        {
            using (MethodStoreContext context = new MethodStoreContext())
            {
                object result = context.Find(typeof(Models.Method), id) as Models.Method;

                return result as T;
            }
        }

        internal void UpdateMethods(T obj)
        {
            using (MethodStoreContext context = new MethodStoreContext())
            {

                if (obj is Models.Method objMethod)
                    if (objMethod.ID == 0)
                        context.Add(objMethod);
                    else
                        context.Update(objMethod);
                else if (obj is Models.Group objGroup)
                    if (objGroup.ID == 0)
                        context.Add(objGroup);
                    else
                        context.Update(objGroup);

                context.SaveChanges();
            }
        }

        internal List<T> GetList()
        {
            List<T> listT = new List<T>();

            using (MethodStoreContext context = new MethodStoreContext())
            {
                Type typeofT = typeof(T);

                if (typeofT == typeof(Models.Group))
                    return context.Groups.ToList() as List<T>;
                else if (typeofT == typeof(Models.Types))
                    return context.Types.ToList() as List<T>;
                else if (typeofT == typeof(Models.RemovingText))
                    return context.RemovingTexts.ToList() as List<T>;
                else
                    throw new NotImplementedException();

            }
        }

        internal bool IsEmpty()
        {
            using (MethodStoreContext context = new MethodStoreContext())
            {
                return !context.RemovingTexts.Any();
            }
        }
    }
}
