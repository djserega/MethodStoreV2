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
        internal List<T> GetListMethods(ParametersSearch parametersSearch)
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
            return methods as List<T>;
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
    }
}
