using OnlineExamProject.Const;
using System.Linq.Expressions;

namespace OnlineExamProject.IRepoistary
{
    public interface IBaseRepoistary<T> where T : class
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);


        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllExpectFirst();
        IEnumerable<T> GetAll(string[] includes);
        Task<IEnumerable<T>> GetAllAsync();

        T Find(Expression<Func<T, bool>> match, string[] includes = null);
        T FindLast(Expression<Func<T, bool>> match, string[] includes = null);
        T FirstItem();
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);


        IEnumerable<T> FindAll(Expression<Func<T, bool>> match, string[] includes = null);

        IEnumerable<T> FindAll(Expression<Func<T, bool>> match, int take, int skip);

        IEnumerable<T> FindAll(Expression<Func<T, bool>> match, int? take, int? skip,
            Expression<Func<T, object>> order_By = null, string orderByDirection =
            orderBy.Ascending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = orderBy.Ascending);

        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> Entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);


        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        void Attach(T entity);
        int Count();
        int Count(Expression<Func<T, bool>> match);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);

    }
}
