namespace Mamba.ViewModels
{
    public class PaginationVM<T> where T : class, new()
    {
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public ICollection<T> Items { get; set; }
        public T Item { get; set; }
    }
}
