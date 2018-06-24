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

                    if (parametersSearch.SearchInGroup)
                        contextMethodsSearch = contextMethods.Where(f => f.Group.Contains(searchText));
                    if (parametersSearch.SearchInType)
                        contextMethodsSearch = contextMethods.Where(f => f.Type.Contains(searchText));
                    if (parametersSearch.SearchInObjectName)
                        contextMethodsSearch = contextMethods.Where(f => f.ObjectName.Contains(searchText));
                    if (parametersSearch.SearchInMethodName)
                        contextMethodsSearch = contextMethods.Where(f => f.MethodName.Contains(searchText));
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
