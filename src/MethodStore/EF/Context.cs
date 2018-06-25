using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MethodStore.EF
{
    internal class Context
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

        internal void RemoveMethods(Models.Method method)
        {
            using (MethodStoreContext context = new MethodStoreContext())
            {
                context.Remove(method);
                context.SaveChanges();
            }
        }

        internal Models.Method FindByID(int id)
        {
            using (MethodStoreContext context = new MethodStoreContext())
            {
                return context.Find(typeof(Models.Method), id) as Models.Method;
            }
        }

        internal void UpdateMethods(Models.Method method)
        {
            using (MethodStoreContext context = new MethodStoreContext())
            {
                if (method.ID == 0)
                    context.Add(method);
                else
                    context.Update(method);

                context.SaveChanges();
            }
        }
    }
}
