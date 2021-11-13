using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateMyAir.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RateMyAir.API.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Generic method to paginate an IQueryable set of data
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <typeparam name="TDtoOut">Type of DTO</typeparam>
        /// <typeparam name="TkeyType">Type of the OrderBy Key</typeparam>
        /// <param name="controller">ControllerBase</param>
        /// <param name="source">IQueryable source of data</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of elements per page</param>
        /// <param name="orderByKey">Key of the OrderBy</param>
        /// <param name="automapperConfig">Automapper configuration</param>
        /// <returns>PagedResponse of type <typeparamref name="TDtoOut"/></returns>
        public static async Task<PagedResponse<IEnumerable<TDtoOut>>> PaginateSourceData<TEntity, TDtoOut, TkeyType>(this ControllerBase controller, IQueryable<TEntity> source, int pageNumber, int pageSize, Expression<Func<TEntity, TkeyType>> orderByKey, IConfigurationProvider automapperConfig)
        {
            int totalRecords = await source.CountAsync();
            var pagedData = await source.OrderBy(orderByKey).Skip((pageNumber - 1) * pageSize).Take(pageSize).ProjectTo<TDtoOut>(automapperConfig).ToListAsync();
            return new PagedResponse<IEnumerable<TDtoOut>>(pagedData, pageNumber, pageSize, totalRecords);
        }

        /// <summary>
        /// Get the full url of the domain where the application is hosted (ie. https://mydomain.com)
        /// </summary>
        /// <param name="controller">ControllerBase</param>
        /// <returns>Full url of the domain</returns>
        public static string GetWebsiteDomain(this ControllerBase controller)
        {
            string hostname = controller.Request.Host.Host;
            string port = controller.Request.Host.Port?.ToString() ?? "80";
            string protocol = "http";
            try
            {
                //protocol = controller.Request.Protocol.Split("/")[0].ToLower();
                protocol = controller.Request.Scheme;

                if (controller.Request.IsHttps && !protocol.ToLower().EndsWith("s"))
                    protocol = protocol + "s";
            }
            catch
            {
            }

            if (hostname == "localhost") return protocol + "://" + hostname + ":" + port;
            else return protocol + "://" + hostname;
        }

    }
}
