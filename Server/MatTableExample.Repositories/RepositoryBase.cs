using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace MatTableExample.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string[] _sortDirections = { "ASC", "DESC" };

        protected RepositoryBase(): this(ConfigurationManager.ConnectionStrings["MainDb"].ConnectionString) { }
        protected RepositoryBase(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string ConnectionString { get; }

        protected string GetPaginationQuery(int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            if (!AllowedSortColumns.Contains(sortColumn, StringComparer.InvariantCultureIgnoreCase))
                throw new ArgumentException("Not allowed columns", nameof(sortColumn));
            if (!_sortDirections.Contains(sortDirection, StringComparer.InvariantCultureIgnoreCase))
                throw new ArgumentException("Invalid sort direction", nameof(sortDirection));


            return $@"
                ORDER BY {sortColumn} {sortDirection} 
                OFFSET {pageIndex * pageSize} 
                ROWS FETCH NEXT {pageSize} 
                ROWS ONLY;";
        }

        protected abstract IEnumerable<string> AllowedSortColumns { get; }
    }

    public class PaginationQueryBuilder
    {
        private readonly string[] _sortDirections = { "ASC", "DESC" };

        public string GetPaginationQuery(int pageIndex, int pageSize, string sortColumn, string sortDirection, IEnumerable<string> allowedSortColumns)
        {
            if (!allowedSortColumns.Contains(sortColumn, StringComparer.InvariantCultureIgnoreCase))
                throw new ArgumentException("Not allowed columns", nameof(sortColumn));
            if (!_sortDirections.Contains(sortDirection, StringComparer.InvariantCultureIgnoreCase))
                throw new ArgumentException("Invalid sort direction", nameof(sortDirection));


            return $@"
                ORDER BY {sortColumn} {sortDirection} 
                OFFSET {pageIndex * pageSize} 
                ROWS FETCH NEXT {pageSize} 
                ROWS ONLY;";
        }
    }
}
