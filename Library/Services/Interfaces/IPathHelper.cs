namespace Library.Services.Interfaces
{
    public interface IPathHelper
    {
        string GetPathToBookCover(int bookId);
        bool? IsBookCoverExist(int bookId);
    }
}